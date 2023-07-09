using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : PlayerBaseState
{
    public readonly float dashPower;
    public readonly float dashSpeed;

    public DashState(PlayerController controller) : base(controller)
    {
        dashPower = 5f;
        dashSpeed = 8f;
    }

    public override void Enter()
    {
        Player.Instance.animator.applyRootMotion = false;
        Controller.StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        Player.Instance.animator.SetBool("Dash", true);
        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
        Vector3 moveVec = (forwardVec * Controller.moveDir.z + rightVec * Controller.moveDir.x);

        Vector3 dashDirection = (Controller.moveDir == Vector3.zero) ? Player.Instance.transform.forward : moveVec;

        Quaternion lookRotation = Quaternion.LookRotation(dashDirection);
        Player.Instance.transform.rotation = Quaternion.Lerp(Player.Instance.transform.rotation, lookRotation, 0.2f);

        Vector3 start = Controller.transform.position;
        Vector3 end = start + dashDirection * dashPower;

        float rate = 0f;
        float totalTime = Vector3.Distance(start, end) / dashSpeed;

        while (rate < 1f)
        {
            rate += Time.deltaTime / totalTime;

            Vector3 targetPosition = Vector3.Lerp(start, end, rate);
            Vector3 moveDirection = (targetPosition - Controller.transform.position).normalized;

            Player.Instance.controller.Move(moveDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }

        Player.Instance.animator.SetBool("Dash", false);
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        Player.Instance.animator.applyRootMotion = true;
        Player.Instance.animator.SetBool("Dash", false);
        Controller.StopCoroutine(DashRoutine());
    }
}