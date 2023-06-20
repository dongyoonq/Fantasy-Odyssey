using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackState : PlayerBaseState
{
    readonly int hashIsChargingAttack = Animator.StringToHash("IsChargingAttack");
    Weapon useWeapon;

    public ChargeAttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        useWeapon = Player.Instance.playerController.GetWeapon();
        Player.Instance.animator.SetBool(hashIsChargingAttack, true);
        useWeapon.ComboCount = 0;
        useWeapon.ChargingAttack();
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