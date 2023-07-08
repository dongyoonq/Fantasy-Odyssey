using SpiderState;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.UI.GridLayoutGroup;

public class Spider : Monster, IHitable
{
    public enum State { Idle, Trace, Return, BiteAttack, ProjecTileAttack, TakeDamage, Die, Size }

    [NonSerialized] public Vector3 spawnPos;
    [NonSerialized] public Animator animator;

    [NonSerialized] public float ProjecttileTime;
    [NonSerialized] public float ySpeed;

    List<MonsterBaseState<Spider>> states;
    public Coroutine projectTileAttackRoutine;
    public Coroutine projectTileMoveRoutine;
    public Coroutine biteRoutine;
    public Coroutine takedamageRoutine;
    public CharacterController controller;
    State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        states = new List<MonsterBaseState<Spider>>((int)State.Size)
        {
            new SpiderState.IdleState(this),
            new SpiderState.TraceState(this),
            new SpiderState.ReturnState(this),
            new SpiderState.BiteAttackState(this),
            new SpiderState.ProjectTileAttackState(this),
            new SpiderState.TakeDamageState(this),
            new SpiderState.DieState(this),
        };

        data = Resources.Load<BaseMonsterData>("Data/MonsterData/Spider/Spider");
    }

    private void OnEnable()
    {
        currHp = data.maxHp;
        spawnPos = transform.position;
    }

    private void Start()
    {
        currState = State.Idle;
        states[(int)currState]?.Enter();
    }

    private void Update()
    {
        if (Player.Instance == null)
        {
            StopAllCoroutines();
            return;
        }

        Fall();
        states[(int)currState]?.Update();
    }

    public void ChangeState(State state)
    {
        if (currState == State.Die)
            return;

        states[(int)currState]?.Exit();
        currState = state;
        states[(int)currState]?.Enter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SpiderArea")
        {
            if (projectTileAttackRoutine != null) StopCoroutine(projectTileAttackRoutine);
            if (biteRoutine != null) StopCoroutine(biteRoutine);
            ChangeState(State.Return);
        }
    }

    void Fall()
    {
        if (IsGrounded() && ySpeed < 0)
            ySpeed = -2;
        else
            ySpeed += Physics.gravity.y * Time.deltaTime;

        if (controller.enabled)
            controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.3f, transform.lossyScale.z);
        return Physics.CheckBox(transform.position, boxSize, Quaternion.identity,
               LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.agressiveMonsterData[0].detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.meleeMonsterData[0].detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.rangeMonsterData[0].detectRange);
    }

    public void Hit(int damage)
    {
        if (currState == State.Die)
            return;

        if (biteRoutine != null && projectTileAttackRoutine != null)
        {
            StopCoroutine(projectTileAttackRoutine);
            StopCoroutine(biteRoutine);
        }    

        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);

        if (currState == State.TakeDamage)
            StartCoroutine(coolTimer());
        else
            ChangeState(State.TakeDamage);

        currHp -= damage;
        GameManager.Ui.SetFloating(gameObject, -damage);
        GameManager.Sound.PlaySFX("MonsterHit");

        if (currHp <= 0)
        {
            if (biteRoutine != null && projectTileAttackRoutine != null && takedamageRoutine != null && projectTileMoveRoutine != null)
            {
                StopCoroutine(projectTileAttackRoutine);
                StopCoroutine(biteRoutine);
                StopCoroutine(takedamageRoutine);
                StopCoroutine(projectTileMoveRoutine);
            }

            animator.SetBool("Move", false);
            animator.SetBool("Attack", false);
            animator.SetBool("TakeDamage", false);
            ChangeState(State.Die);
        }
    }

    IEnumerator coolTimer()
    {
        states[(int)currState]?.Exit();
        yield return new WaitForSeconds(0.1f);
        states[(int)currState]?.Enter();
    }

    int[] percent = Enumerable.Range(1, 100).ToArray();

    // 0 - SpiderBooty
    public override void DropItemAndUpdateExp()
    {
        StartCoroutine(ExpDropRoutine());

        int random = UnityEngine.Random.Range(1, 101);

        // 전리품(Spider Booty) 드랍확률 70%
        int spiderBootyDropPercent = (int)(percent.Length * 0.7f);
        for (int i = 0; i < spiderBootyDropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[0].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[0];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        random = UnityEngine.Random.Range(1, 101);

        // 장비(Sword) 드랍확률 5%
        int spiderSwordDropPercent = (int)(percent.Length * 0.05f);
        for (int i = 0; i < spiderSwordDropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[1].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[1];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        IEnumerator ExpDropRoutine()
        {
            for (int i = 0; i < data.dropExp / 5; i++)
            {
                Player.Instance.Exp += 5;
                yield return new WaitForSeconds(0.00001f);
            }
        }
    }
}
