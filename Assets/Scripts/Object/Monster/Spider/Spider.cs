using SpiderState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour, IHitable
{
    public enum State { Idle, Trace, Attack, TakeDamage, ProjectileAttack, CastAttack, Die, Size }

    [SerializeField] public float detectRange;
    [SerializeField] public float biteAttackRange;
    [SerializeField] public float projectileAttackRange;
    [SerializeField] public int health;

    [NonSerialized] public Vector3 spawnPos;
    [NonSerialized] public Animator animator;
    [NonSerialized] public float coolTime;

    List<MonsterBaseState<Spider>> states;
    public State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        states = new List<MonsterBaseState<Spider>>((int)State.Size)
        {
            new SpiderState.IdleState(this),
            new SpiderState.TraceState(this),
            new SpiderState.AttackState(this),
            new SpiderState.TakeDamageState(this),
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
            animator.SetBool("Attack", false);
            ChangeState(State.Idle);
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

    public void Hit(int damamge)
    {
        health -= damamge;
        ChangeState(State.TakeDamage);
    }
}
