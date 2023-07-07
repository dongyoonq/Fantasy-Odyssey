using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class DemonBoss : Monster, IHitable
{
    public enum State { Spawn, Idle, Move, Claw, Smash, Breathe, JumpAttack, Grab, Summon, Heal, Throw, Rage, Groggy, Die, Size }

    [NonSerialized] public Animator animator;
    [NonSerialized] public float rockElapseTime;
    [NonSerialized] public bool isGroggyed;
    [NonSerialized] public int stunValue;
    [NonSerialized] public CharacterController controller;
    [NonSerialized] public float ySpeed;
    [NonSerialized] public DemonBomb summon1;
    [NonSerialized] public DemonBomb summon2;

    public int stunThreshold;

    [SerializeField] public float patternChangeTimer = 0f;
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
        controller = GetComponent<CharacterController>();
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
        if (Player.Instance == null)
        {
            StopAllCoroutines();
            return;
        }

        Fall();

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

        states[(int)currState]?.Exit();
        currState = state;
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
        //ChangeState(State.JumpAttack); // : State Test
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

    int[] percent = Enumerable.Range(1, 100).ToArray();

    // 0 - Demon's Heart,  1 - Golden Sentinel Mail, 2 - Lightstep Shoes, 3 - Evergreen Cloak, 4 - Flameforged Gauntlets
    // 5 - Rune-Ward Helm, 6 - Flameheart Leggings
    public override void DropItemAndUpdateExp()
    {
        StartCoroutine(ExpDropRoutine());

        int random = UnityEngine.Random.Range(1, 101);

        // 전리품(Demon's Heart) 드랍확률 100%
        int dropPercent = (int)(percent.Length * 1f);
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

        // Golden Sentinel Mail 드랍확률 100%
        dropPercent = (int)(percent.Length * 1f);
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

        // Lightstep Shoes 드랍확률 100%
        dropPercent = (int)(percent.Length * 1f);
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

        // Evergreen Cloak 드랍확률 100%
        dropPercent = (int)(percent.Length * 1f);
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

        // Flameforged Gauntlets 드랍확률 100%
        dropPercent = (int)(percent.Length * 1f);
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

        // Rune-Ward Helm 드랍확률 100%
        dropPercent = (int)(percent.Length * 1f);
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

        // Flameheart Leggings 드랍확률 100%
        dropPercent = (int)(percent.Length * 1f);
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