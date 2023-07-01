using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class DemonBoss : Monster, IHitable
{
    public enum State { Spawn, Idle, Move, Claw, Smash, Breathe, JumpAttack, Grab, Summon, Throw, Heal, TakeDamage, Groggy, Die, Rage, Size }

    [NonSerialized] public Animator animator;
    [NonSerialized] public float patternChangeTimer = 0f;
    [NonSerialized] public float rockElapseTime;
    public int stunValue;

    [SerializeField] public GameObject jaw;
    [SerializeField] public GameObject lefthand;
    [SerializeField] public GameObject righthand;
    [SerializeField] public Transform summonPos1;
    [SerializeField] public Transform summonPos2;

    public float patternChangeInterval = 8f;
    public float coolTime;

    public Coroutine clawRoutine;
    public Coroutine smashRoutine;
    public Coroutine breatheRoutine;
    public Coroutine hitBoxRoutine;
    public Coroutine jumpAttackRoutine;
    public Coroutine grabAttackRoutine;
    public Coroutine summonAttackRoutine;
    public Coroutine rockAttackRoutine;

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
            new Demon_Boss.ThrowAttackState(this),
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

        if (currState != State.Spawn)
        {
            patternChangeTimer += Time.deltaTime;
            if (patternChangeTimer >= patternChangeInterval)
            {
                patternChangeTimer = 0f;
                ChangePatternState();
            }
        }
    }

    public void ChangeState(State state)
    {
        if (currState == State.Die)
            return;

        states[(int)currState]?.Exit();
        currState = state;
        states[(int)currState]?.Enter();
    }

    private void ChangePatternState()
    {
        int patternIndex = UnityEngine.Random.Range((int)State.Smash, (int)State.Summon + 1);
        ChangeState((State)patternIndex);
        //ChangeState(State.Throw); // : State Test
    }

    public override void DropItemAndUpdateExp()
    {
        // 드랍 구현
    }

    public void Hit(int damamge)
    {
        // 맞기 구현
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