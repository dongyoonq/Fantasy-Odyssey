using SpiderState;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : MonoBehaviour, IHitable
{
    public enum State { Idle, Trace, Return, Attack, TakeDamage, Die, ProjectileAttack, CastAttack, Size }

    [SerializeField] public float detectRange;
    [SerializeField] public float biteAttackRange;
    [SerializeField] public float projectileAttackRange;
    [SerializeField] public int health;
    [SerializeField] public int biteAttackDamage;

    [NonSerialized] public Vector3 spawnPos;
    [NonSerialized] public Animator animator;
    [NonSerialized] public float coolTime;
    [NonSerialized] public SpiderSpawn spawnInfo;

    List<MonsterBaseState<Spider>> states;
    State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        states = new List<MonsterBaseState<Spider>>((int)State.Size)
        {
            new SpiderState.IdleState(this),
            new SpiderState.TraceState(this),
            new SpiderState.ReturnState(this),
            new SpiderState.AttackState(this),
            new SpiderState.TakeDamageState(this),
            new SpiderState.DieState(this),
        };
    }

    private void OnEnable()
    {
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
        Debug.Log(currState + "Exit");
        states[(int)currState]?.Exit();
        currState = state;
        Debug.Log(currState + "Enter");
        states[(int)currState]?.Enter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SpiderArea")
        {
            StopAllCoroutines();
            ChangeState(State.Return);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, biteAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, projectileAttackRange);
    }

    public void Hit(int damage)
    {
        StopAllCoroutines();
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);

        if (currState == State.TakeDamage)
            StartCoroutine(coolTimer());
        else
            ChangeState(State.TakeDamage);

        health -= damage;

        if (health <= 0)
        {
            StopAllCoroutines();
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
}
