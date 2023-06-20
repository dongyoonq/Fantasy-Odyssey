using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightAttackState : BaseState
{
    readonly int hashIsRightAttack = Animator.StringToHash("IsRightAttack");
    Weapon useWeapon;

    public RightAttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        useWeapon = Player.Instance.playerController.GetWeapon();
        Player.Instance.animator.SetBool(hashIsRightAttack, true);
        useWeapon.ComboCount = 0;
        useWeapon.RightAttack();
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