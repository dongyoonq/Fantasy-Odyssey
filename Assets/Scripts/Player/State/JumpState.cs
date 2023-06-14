using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class JumpState : BaseState
{
    public JumpState(PlayerController controller) : base(controller)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            Jump();
    }

    void Jump()
    {
        Player.Instance.YSpeed = Player.Instance.JumpPower;
    }


    public override void Enter()
    {

    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        Player.Instance.YSpeed = 0f;
    }
}
