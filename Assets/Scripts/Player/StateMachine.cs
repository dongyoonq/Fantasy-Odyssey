using System.Collections.Generic;
using UnityEngine;

public enum StateName
{
    MOVE,
    Dash,
    ATTACK,
    LAttack,
    RAttack,
    DashAttack,
    ChargeAttack,
    SkillAttack,
    UltAttack,
    Die,
}

public class StateMachine
{
    public PlayerBaseState CurrentState { get; private set; }  // 현재 상태
    private Dictionary<StateName, PlayerBaseState> states =
                                        new Dictionary<StateName, PlayerBaseState>();
    public bool onDied;


    public StateMachine(StateName stateName, PlayerBaseState state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(StateName stateName, PlayerBaseState state)  // 상태 등록
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public PlayerBaseState GetState(StateName stateName)  // 상태 꺼내오기
    {
        if (states.TryGetValue(stateName, out PlayerBaseState state))
            return state;
        return null;
    }

    public void DeleteState(StateName removeStateName)  // 상태 삭제
    {
        if (states.ContainsKey(removeStateName))
        {
            states.Remove(removeStateName);
        }
    }

    public void ChangeState(StateName nextStateName)    // 상태 전환
    {
        if (onDied)
            return;

        CurrentState?.Exit();   //현재 상태를 종료하는 메소드를 실행하고,
        if (states.TryGetValue(nextStateName, out PlayerBaseState newState)) // 상태 전환
        {
            CurrentState = newState;
        }
        CurrentState?.Enter();  // 다음 상태 진입 메소드 실행
    }

    public void UpdateState()
    {
        CurrentState?.Update();
    }

    public void FixedUpdateState()
    {
        CurrentState?.FixedUpdate();
    }
}