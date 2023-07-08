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
    [NonSerialized] public CharacterController controller;

    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstacleMask;

    List<MonsterBaseState<StingBee>> states;
    public Coroutine attackRoutine;
    public Coroutine takedamageRoutine;

    State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
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
        if (Player.Instance == null)
        {
            StopAllCoroutines();
            return;
        }

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
        GameManager.Sound.PlaySFX("MonsterHit");

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

        if (currState == State.Idle || currState == State.Trace || currState == State.Return)
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

    // 0 - Venomous Sting,  1 - Hero's Plate, 2 - Windrunner Boots, 3 - Shadowwave Cloak, 4 - Ironclaw Gauntlets
    // 5 - Warrior's Crown, 6 - Shadowweave Trousers
    public override void DropItemAndUpdateExp()
    {
        StartCoroutine(ExpDropRoutine());

        int random = UnityEngine.Random.Range(1, 101);

        // 전리품(Venomous Sting) 드랍확률 70%
        int dropPercent = (int)(percent.Length * 0.7f);
        for (int i = 0; i < dropPercent; i++)
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

        // Hero's Plate 드랍확률 2%
        dropPercent = (int)(percent.Length * 0.02f);
        for (int i = 0; i < dropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[1].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[1];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        random = UnityEngine.Random.Range(1, 101);

        // Windrunner Boots 드랍확률 2%
        dropPercent = (int)(percent.Length * 0.02f);
        for (int i = 0; i < dropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[2].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[2];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        random = UnityEngine.Random.Range(1, 101);

        // Shadowwave Cloak 드랍확률 2%
        dropPercent = (int)(percent.Length * 0.02f);
        for (int i = 0; i < dropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[3].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[3];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        random = UnityEngine.Random.Range(1, 101);

        // Ironclaw Gauntlets 드랍확률 2%
        dropPercent = (int)(percent.Length * 0.02f);
        for (int i = 0; i < dropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[4].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[4];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        random = UnityEngine.Random.Range(1, 101);

        // Warrior's Crown 드랍확률 2%
        dropPercent = (int)(percent.Length * 0.02f);
        for (int i = 0; i < dropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[5].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[5];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        random = UnityEngine.Random.Range(1, 101);

        // Shadowweave Trousers 드랍확률 2%
        dropPercent = (int)(percent.Length * 0.02f);
        for (int i = 0; i < dropPercent; i++)
        {
            if (percent[i] == random)
            {
                Item fieldItem = Instantiate(data.dropTable[6].prefab, transform.position + (transform.up * 0.5f), Quaternion.identity);
                ItemData tempData = data.dropTable[6];
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;
            }
        }

        IEnumerator ExpDropRoutine()
        {
            for (int i = 0; i < data.dropExp / 10; i++)
            {
                Player.Instance.Exp += 10;
                yield return new WaitForSeconds(0.00001f);
            }
        }
    }
}
