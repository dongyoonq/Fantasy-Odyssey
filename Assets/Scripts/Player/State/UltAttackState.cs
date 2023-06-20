using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttackState : BaseState
{
    readonly int hashIsUltSkillAtaack = Animator.StringToHash("IsUltAttack");
    Weapon useWeapon;

    public UltAttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        useWeapon = Player.Instance.playerController.GetWeapon();
        Player.Instance.animator.SetBool(hashIsUltSkillAtaack, true);
        Player.Instance.playerController.isUltAttack = false;
        useWeapon.ComboCount = 0;
        useWeapon.UltimateSkill();
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