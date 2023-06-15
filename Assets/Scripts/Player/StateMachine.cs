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
    public BaseState CurrentState { get; private set; }  // ���� ����
    private Dictionary<StateName, BaseState> states =
                                        new Dictionary<StateName, BaseState>();


    public StateMachine(StateName stateName, BaseState state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(StateName stateName, BaseState state)  // ���� ���
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public BaseState GetState(StateName stateName)  // ���� ��������
    {
        if (states.TryGetValue(stateName, out BaseState state))
            return state;
        return null;
    }

    public void DeleteState(StateName removeStateName)  // ���� ����
    {
        if (states.ContainsKey(removeStateName))
        {
            states.Remove(removeStateName);
        }
    }

    public void ChangeState(StateName nextStateName)    // ���� ��ȯ
    {
        CurrentState?.Exit();   //���� ���¸� �����ϴ� �޼ҵ带 �����ϰ�,
        if (states.TryGetValue(nextStateName, out BaseState newState)) // ���� ��ȯ
        {
            CurrentState = newState;
        }
        CurrentState?.Enter();  // ���� ���� ���� �޼ҵ� ����
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