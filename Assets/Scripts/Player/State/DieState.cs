using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DieState : PlayerBaseState
{
    public DieState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Player.Instance.StopAllCoroutines();
        Player.Instance.animator.SetBool("Die", true);
        Player.Instance.GetComponent<PlayerInput>().enabled = false;
        Player.Instance.controller.enabled = false;
        Player.Instance.stateMachine.onDied = true;
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        Player.Instance.animator.SetBool("Die", false);
        Player.Instance.GetComponent<PlayerInput>().enabled = true;
        Player.Instance.controller.enabled = true;
    }
}