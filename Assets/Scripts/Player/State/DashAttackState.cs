using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackState : PlayerBaseState
{
    readonly int hashIsDashAttack = Animator.StringToHash("IsDashAttack");
    Weapon useWeapon;

    public DashAttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        useWeapon = Player.Instance.playerController.GetWeapon();
        Player.Instance.animator.SetBool("Dash", false);
        Player.Instance.animator.SetBool(hashIsDashAttack, true);
        useWeapon.ComboCount = 0;
        useWeapon.DashAttack();
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {

    }
}