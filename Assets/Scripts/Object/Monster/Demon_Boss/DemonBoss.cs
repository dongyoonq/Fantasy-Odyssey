using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class DemonBoss : Monster, IHitable
{
    public enum State { Spawn, Idle, Move, Claw, Smash, Breathe, JumpAttack, Grab, Summon, Heal, Throw, Rage, TakeDamage, Groggy, Die, Size }

    [NonSerialized] public Animator animator;
    [NonSerialized] public float patternChangeTimer = 0f;
    [NonSerialized] public float rockElapseTime;
    [NonSerialized] int stunValue;
    public int stunThreshold;

    [SerializeField] public GameObject jaw;
    [SerializeField] public GameObject lefthand;
    [SerializeField] public GameObject righthand;
    [SerializeField] public Transform summonPos1;
    [SerializeField] public Transform summonPos2;

    public float patternChangeInterval = 12f;
    public float coolTime;

    public bool pharse2;

    public Coroutine clawRoutine;
    public Coroutine smashRoutine;
    public Coroutine breatheRoutine;
    public Coroutine hitBoxRoutine;
    public Coroutine jumpAttackRoutine;
    public Coroutine grabAttackRoutine;
    public Coroutine summonAttackRoutine;
    public Coroutine rockAttackRoutine;
    public Coroutine healRoutine;
    public Coroutine rageRoutine;

    List<MonsterBaseState<DemonBoss>> states;
    State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        states = new List<MonsterBaseState<DemonBoss>>((int)State.Size)
        {
            new Demon_Boss.SpawnState(this),
            new Demon_Boss.IdleState(this),
            new Demon_Boss.MoveState(this),
            new Demon_Boss.ClawAttackState(this),
            new Demon_Boss.SmashAttackState(this),
            new Demon_Boss.BreatheAttackState(this),
            new Demon_Boss.JumpAttackState(this),
            new Demon_Boss.GrabAttackState(this),
            new Demon_Boss.SummonAttackState(this),
            new Demon_Boss.HealState(this),
            new Demon_Boss.ThrowAttackState(this),
            new Demon_Boss.RageState(this),
        };
    }

    private void OnEnable()
    {
        currHp = data.maxHp;
    }

    private void Start()
    {
        currState = State.Spawn;
        states[(int)currState]?.Enter();
    }

    private void Update()
    {
        states[(int)currState]?.Update();

        if (currState != State.Spawn && currState != State.Idle)
        {
            patternChangeTimer += Time.deltaTime;
            if (patternChangeTimer >= patternChangeInterval)
            {
                ChangePatternState();
            }
        }
    }

    public void ChangeState(State state)
    {
        if (currState == State.Die)
            return;

        Debug.Log(currState);
        states[(int)currState]?.Exit();
        currState = state;
        Debug.Log(currState);
        states[(int)currState]?.Enter();
    }

    private void ChangePatternState()
    {
        if (currState != State.Move)
            return;

        int patternIndex;
        if (!pharse2)
            patternIndex = UnityEngine.Random.Range((int)State.Smash, (int)State.Summon + 1);
        else
            patternIndex = UnityEngine.Random.Range((int)State.Smash, (int)State.Heal + 1);

        ChangeState((State)patternIndex);
        patternChangeTimer = 0f;
        //ChangeState(State.Heal); // : State Test
    }

    public override void DropItemAndUpdateExp()
    {
        // 드랍 구현
    }

    public void Hit(int damamge)
    {
        currHp -= damamge;
        stunValue += 10;

        if (currHp <= 0)
        {
            ChangeState(State.Die);
            return;
        }

        if (stunValue >= stunThreshold)
        {
            ChangeState(State.Groggy);
            return;
        }

        // 맞기 구현
        if (currHp < data.maxHp * 0.4f && !pharse2)
        {
            pharse2 = true;
            ChangeState(State.Rage);
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 12f);

        //Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        //Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
        //Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
        //Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }
}