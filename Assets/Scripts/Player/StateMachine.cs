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
}

public class StateMachine
{
    public PlayerBaseState CurrentState { get; private set; }  // ���� ����
    private Dictionary<StateName, PlayerBaseState> states =
                                        new Dictionary<StateName, PlayerBaseState>();


    public StateMachine(StateName stateName, PlayerBaseState state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(StateName stateName, PlayerBaseState state)  // ���� ���
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public PlayerBaseState GetState(StateName stateName)  // ���� ��������
    {
        if (states.TryGetValue(stateName, out PlayerBaseState state))
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
        if (states.TryGetValue(nextStateName, out PlayerBaseState newState)) // ���� ��ȯ
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