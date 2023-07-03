using SpiderState;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class StingBee : Monster, IHitable, IHearable
{
    public enum State { Idle, Trace, Return, Attack, TakeDamage, Die, Size }

    [NonSerialized] public Vector3 spawnPos;
    [NonSerialized] public Animator animator;
    [NonSerialized] public float coolTime;

    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstacleMask;

    List<MonsterBaseState<StingBee>> states;
    public Coroutine attackRoutine;
    public Coroutine takedamageRoutine;

    State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        states = new List<MonsterBaseState<StingBee>>((int)State.Size)
        {
            new StingBeeState.IdleState(this),
            new StingBeeState.TraceState(this),
            new StingBeeState.ReturnState(this),
            new StingBeeState.AttackState(this),
            new StingBeeState.TakeDamageState(this),
            new StingBeeState.DieState(this),
        };
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
        if (other.gameObject.name == "StingBeeArea")
        {
            if (attackRoutine != null) StopCoroutine(attackRoutine);
            ChangeState(State.Return);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.agressiveMonsterData[0].detectRange);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + data.agressiveMonsterData[0].detectAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - data.agressiveMonsterData[0].detectAngle * 0.5f);
        Debug.DrawRay(transform.position, rightDir * data.agressiveMonsterData[0].detectRange, Color.yellow);
        Debug.DrawRay(transform.position, leftDir * data.agressiveMonsterData[0].detectRange, Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }

    public void Hit(int damage)
    {
        if (currState == State.Die)
            return;

        currHp -= damage;
        GameManager.Ui.SetFloating(gameObject, -damage);

        if (currHp <= 0)
        {
            StopAllCoroutines();
            animator.SetBool("Move", false);
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
            animator.SetBool("Damage", false);
            ChangeState(State.Die);
        }

        if (currState == State.Idle || currState == State.Trace)
        {
            ChangeState(State.TakeDamage);
        }
    }

    public void Hear(Transform source)
    {
        if (currState == State.TakeDamage || currState == State.Die || currState == State.Attack)
            return;

        ChangeState(State.Trace);
    }

    int[] percent = Enumerable.Range(1, 100).ToArray();

    // 0 - SpiderBooty,  1 - NormalSword
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
