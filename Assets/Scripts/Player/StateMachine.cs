using System.Collections.Generic;
using UnityEngine;

public enum StateName
{
    MOVE,
    ATTACK,
    Dash,
    DashAttack,
}

public class StateMachine
{
    public BaseState CurrentState { get; private set; }  // 현재 상태
    private Dictionary<StateName, BaseState> states =
                                        new Dictionary<StateName, BaseState>();


    public StateMachine(StateName stateName, BaseState state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(StateName stateName, BaseState state)  // 상태 등록
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public BaseState GetState(StateName stateName)  // 상태 꺼내오기
    {
        if (states.TryGetValue(stateName, out BaseState state))
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
        CurrentState?.Exit();   //현재 상태를 종료하는 메소드를 실행하고,
        if (states.TryGetValue(nextStateName, out BaseState newState)) // 상태 전환
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