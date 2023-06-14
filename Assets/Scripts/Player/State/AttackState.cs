using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public static bool IsAttack = false;
    public const float CanReInputTime = 1.2f;

    public AttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        IsAttack = true;
        Controller.GetWeapon()?.Use();
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