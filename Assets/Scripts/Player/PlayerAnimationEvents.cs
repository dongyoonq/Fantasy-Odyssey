using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void OnFinishedAttack()
    {
        AttackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsLeftAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }

    public void OnFineshedRightAttack()
    {
        AttackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsRightAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }

    public void OnFinishedDashAttack()
    {
        AttackState.IsLeftAttack = false;
        Player.Instance.animator.applyRootMotion = false;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);

        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }

    public void OnFinishedChargingAttack()
    {
        AttackState.IsLeftAttack = false;
        Player.Instance.animator.applyRootMotion = false;
        Player.Instance.animator.SetBool("IsChargingAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDash()
    {
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinshedJump()
    {
        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }
}