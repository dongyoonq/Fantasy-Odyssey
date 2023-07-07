using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Phantom : Monster, IHitable
{
    public enum State { Idle, Roaming, Trace, Return, Attack, SpellAttack, TakeDamage, Die, Size }

    [SerializeField] public RoamingPoint[] roamingPoint;

    [NonSerialized] public Vector3 spawnPos;
    [NonSerialized] public Animator animator;
    [NonSerialized] public int prevPoint;
    [NonSerialized] public float coolTime;
    [NonSerialized] public CharacterController controller;

    List<MonsterBaseState<Phantom>> states;
    State currState;

    public Coroutine roamingCoolTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        states = new List<MonsterBaseState<Phantom>>((int)State.Size)
        {
            new PhantomState.IdleState(this),
            new PhantomState.RoamingState(this),
            new PhantomState.TraceState(this),
            new PhantomState.ReturnState(this),
            new PhantomState.AttackState(this),
            new PhantomState.SpellAttackState(this),
            new PhantomState.DamageState(this),
            new PhantomState.DieState(this),
        };
    }

    private void OnEnable()
    {
        currHp = data.maxHp;
        spawnPos = transform.position;
        roamingPoint = GameObject.Find("PhantomArea").GetComponentsInChildren<RoamingPoint>();
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

        if (currState == State.Attack || currState == State.SpellAttack || currState == State.Die || currState == State.TakeDamage) 
            if (roamingCoolTime != null)
                StopCoroutine(roamingCoolTime);

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
            animator.SetBool("AttackLeft", false);
            animator.SetBool("AttackRight", false);
            animator.SetBool("Damage", false);
            ChangeState(State.Die);
        }

        if (currState == State.Idle || currState == State.Trace || currState == State.Roaming || currState == State.Return)
        {
            ChangeState(State.TakeDamage);
        }
    }

    int[] percent = Enumerable.Range(1, 100).ToArray();

    // 0 - Spectral Eye,  1 - Stormguard Armor, 2 - Stealthwalkers Dust, 3 - Sunstrike Cloak, 4 - Lionheart Gauntlets
    // 5 - Sunstrike Headguard, 6 - Elder Raven Leggings
    public override void DropItemAndUpdateExp()
    {
        StartCoroutine(ExpDropRoutine());

        int random = UnityEngine.Random.Range(1, 101);

        // 전리품(Spectral Eye) 드랍확률 70%
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

        // Stormguard Armor 드랍확률 2%
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

        // Stealthwalkers Dust 드랍확률 2%
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

        // Sunstrike Cloak 드랍확률 2%
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

        // Lionheart Gauntlets 드랍확률 2%
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

        // Sunstrike Headguard 드랍확률 2%
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

        // Elder Raven Leggings 드랍확률 2%
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
            for (int i = 0; i < data.dropExp / 30; i++)
            {
                Player.Instance.Exp += 30;
                yield return new WaitForSeconds(0.00001f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PhantomArea")
        {
            if (currState != State.Die)
                StopAllCoroutines();
            ChangeState(State.Return);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.agressiveMonsterData[0].detectRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, data.rangeMonsterData[0].detectRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, data.meleeMonsterData[0].detectRange);
    }
}