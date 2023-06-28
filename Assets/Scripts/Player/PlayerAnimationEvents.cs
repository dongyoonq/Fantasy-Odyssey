using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnimationEvents : MonoBehaviour
{
    BaseAttackState attackState;

    private void Start()
    {
        attackState = Player.Instance.stateMachine.GetState(StateName.ATTACK) as BaseAttackState;
    }

    public void OnFinishedAttack()
    {
        attackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsLeftAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFineshedRightAttack()
    {
        attackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsRightAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDashAttack()
    {
        attackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedChargingAttack()
    {
        attackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsChargingAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedSkillAttack()
    {
        attackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsSkillAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedUltSkillAttack()
    {
        attackState.IsLeftAttack = false;
        Player.Instance.animator.SetBool("IsUltAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDash()
    {
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);

        if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
            Player.Instance.inputBuffer.Dequeue();
    }

    public void OnFinshedJump()
    {

    }

    public void FinishTalk()
    {
        Player.Instance.animator.SetBool("Talk", false);
    }
}