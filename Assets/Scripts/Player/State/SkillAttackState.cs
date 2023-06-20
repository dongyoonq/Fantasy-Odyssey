using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackState : PlayerBaseState
{
    Weapon useWeapon;
    readonly int hashIsSkillAtaack = Animator.StringToHash("IsSkillAttack");

    public SkillAttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        useWeapon = Player.Instance.playerController.GetWeapon();

        Player.Instance.animator.SetBool(hashIsSkillAtaack, true);
        useWeapon.ComboCount = 0;
        useWeapon.Skill();
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