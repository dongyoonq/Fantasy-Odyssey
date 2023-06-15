using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.XR;

public class MoveState : BaseState
{
    float orgMoveSpeed;

    public MoveState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        Move();
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        Player.Instance.animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    void Move()
    {
        if (Controller.moveDir.magnitude == 0)
        {
            Player.Instance.animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            return;
        }

        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
        Vector3 moveVec = (forwardVec * Controller.moveDir.z + rightVec * Controller.moveDir.x) * Player.Instance.MoveSpeed * Time.deltaTime;
        Player.Instance.controller.Move(moveVec);

        Quaternion lookRotation = Quaternion.LookRotation(forwardVec * Controller.moveDir.z + rightVec * Controller.moveDir.x);
        Player.Instance.transform.rotation = Quaternion.Lerp(Player.Instance.transform.rotation, lookRotation, 0.2f);

        float percent = ((Controller.OnRunKey) ? 1 : 0.5f) * Controller.moveDir.magnitude;
        Player.Instance.animator.SetFloat("Speed", percent, 0.1f, Time.deltaTime);
    }
}
