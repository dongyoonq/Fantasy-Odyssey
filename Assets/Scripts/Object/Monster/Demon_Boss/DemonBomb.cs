using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonBomb : Monster, IHitable
{
    public enum State { Spawn, Move, Suicide, Size }

    [NonSerialized] public Animator animator;

    List<MonsterBaseState<DemonBomb>> states;
    State currState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        states = new List<MonsterBaseState<DemonBomb>>((int)State.Size)
        {
            new Demon_Bomb.SpawnState(this),
            new Demon_Bomb.MoveState(this),
            new Demon_Bomb.SuicideState(this),
        };
    }

    private void OnEnable()
    {

    }

    private void Start()
    {
        currState = State.Spawn;
        states[(int)currState]?.Enter();
    }

    private void Update()
    {
        states[(int)currState]?.Update();
    }

    public void ChangeState(State state)
    {
        if (currState == State.Suicide)
            return;

        states[(int)currState]?.Exit();
        currState = state;
        states[(int)currState]?.Enter();
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
        Gizmos.DrawWireSphere(transform.position, 4f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 8f);
    }
}