using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftAttackState : BaseState
{
    Coroutine checkAttackReInputCor;
    BaseAttackState attackState;
    Weapon useWeapon;

    public LeftAttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        useWeapon = Player.Instance.playerController.GetWeapon();
        attackState = Player.Instance.playerController.attackState;

        Player.Instance.animator.SetFloat(attackState.hashAttackSpeedAnimation, Player.Instance.Status.AttackSpeed);
        Player.Instance.animator.SetBool(attackState.hashIsLeftAttackAnimation, true);
        Player.Instance.animator.SetInteger(attackState.hashAttackAnimation, ++useWeapon.ComboCount);
        CheckAttackReInput(attackState.CanReInputTime);
        useWeapon.LeftAttack();
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

    public void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            useWeapon.StopCoroutine(checkAttackReInputCor);
        checkAttackReInputCor = useWeapon.StartCoroutine(CheckAttackReInputCoroutine(reInputTime));
    }

    private IEnumerator CheckAttackReInputCoroutine(float reInputTime)
    {
        float currentTime = 0f;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= reInputTime)
                break;


            yield return null;
        }

        useWeapon.ComboCount = 0;
        Player.Instance.animator.SetInteger(attackState.hashAttackAnimation, 0);
    }
}