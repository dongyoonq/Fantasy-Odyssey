using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonBoss : Monster, IHitable
{
    public enum State { Spawn, Idle, Move, Claw, Smash, Breathe, JumpAttack, Grab, Summon, Heal, Throw, Rage, Groggy, Die, Size }

    [NonSerialized] public Animator animator;
    [SerializeField] public float patternChangeTimer = 0f;
    [NonSerialized] public float rockElapseTime;
    [NonSerialized] public bool isGroggyed;
    [NonSerialized] public int stunValue;
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
    public Coroutine groggyRoutine;
    public Coroutine rockIntervalRoutine;

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
            new Demon_Boss.GroggyState(this),
            new Demon_Boss.DieState(this),
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

        if (rockIntervalRoutine != null)
        {
            StopCoroutine(rockIntervalRoutine);
            rockIntervalRoutine = null;
        }

        Debug.Log(currState);
        states[(int)currState]?.Exit();
        currState = state;
        Debug.Log(currState);
        states[(int)currState]?.Enter();
    }

    private void ChangePatternState()
    {
        if (currState == State.Groggy || currState == State.Rage || currState == State.Die)
            return;

        int patternIndex;

        if (!pharse2)
            patternIndex = UnityEngine.Random.Range((int)State.Smash, (int)State.Summon + 1);
        else
            patternIndex = UnityEngine.Random.Range((int)State.Smash, (int)State.Heal + 1);

        ChangeState((State)patternIndex);
        patternChangeTimer = 0f;
        //ChangeState(State.Breathe); // : State Test
    }

    public override void DropItemAndUpdateExp()
    {
        // 드랍 구현
    }

    public void Hit(int damage)
    {
        if (currState == State.Die)
            return;

        currHp -= damage;
        GameManager.Ui.SetFloating(gameObject, -damage);

        if (!isGroggyed)
            stunValue += 10;

        if (currHp <= 0)
        {
            StopAllAnimationCoroutine();
            ChangeState(State.Die);
            return;
        }

        if (stunValue >= stunThreshold && !isGroggyed && !pharse2)
        {
            StopAllAnimationCoroutine();
            ChangeState(State.Groggy);
            isGroggyed = true;
            return;
        }

        // 맞기 구현
        if (currHp < data.maxHp * 0.4f && !pharse2)
        {
            StopAllAnimationCoroutine();
            pharse2 = true;
            ChangeState(State.Rage);
            return;
        }


        if (currState == State.Idle || currState == State.Move)
        {
            animator.SetBool("Claw1", false);
            animator.SetBool("Claw2", false);
            animator.SetBool("Smash", false);
            animator.SetBool("Breath", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Grab", false);
            animator.SetBool("Summon1", false);
            animator.SetBool("Summon2", false);
            animator.SetBool("Throw", false);
            animator.SetBool("Heal", false);
            animator.SetBool("TakeDamage", true);
        }
    }

    public void FinishedDamageAnimation()
    {
        animator.SetBool("TakeDamage", false);
    }

    void StopAllAnimationCoroutine()
    {
        StopAllCoroutines();
        animator.SetBool("Claw1", false);
        animator.SetBool("Claw2", false);
        animator.SetBool("Smash", false);
        animator.SetBool("Breath", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Grab", false);
        animator.SetBool("Summon1", false);
        animator.SetBool("Summon2", false);
        animator.SetBool("Throw", false);
        animator.SetBool("Heal", false);
        animator.SetBool("Groggy", false);
        isGroggyed = false;
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