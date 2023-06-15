using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void OnFinishedAttack()
    {
        AttackState.IsAttack = false;
        Player.Instance.animator.SetBool("IsAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }

    public void OnFinishedDashAttack()
    {
        AttackState.IsAttack = false;
        Player.Instance.animator.applyRootMotion = true;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        //Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDash()
    {
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }

    public void OnFinshedJump()
    {
        if (Player.Instance.inputBuffer.Count != 0)
            Player.Instance.inputBuffer.Dequeue();
    }
}