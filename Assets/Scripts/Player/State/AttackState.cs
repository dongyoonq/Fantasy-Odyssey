using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public static bool IsAttack = false;
    public const float CanReInputTime = 1.2f;
    public static int ComboCount = 0;

    public static readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public static readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public static readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    private Coroutine checkAttackReInputCor;

    public AttackState(PlayerController controller) : base(controller) { }

    public override void Enter()
    {
        IsAttack = true;
        if (Controller.GetWeapon())
            Controller.GetWeapon()?.Use();
        else
            Attack();
    }

    void Attack()
    {
        ComboCount++;
        Player.Instance.animator.SetFloat(hashAttackSpeedAnimation, Player.Instance.Status.AttackSpeed);
        Player.Instance.animator.SetBool(hashIsAttackAnimation, true);
        Player.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(CanReInputTime);
    }

    private void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            Player.Instance.StopCoroutine(checkAttackReInputCor);
        checkAttackReInputCor = Player.Instance.StartCoroutine(CheckAttackReInputCoroutine(reInputTime));
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

        ComboCount = 0;
        Player.Instance.animator.SetInteger(hashAttackAnimation, 0);
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