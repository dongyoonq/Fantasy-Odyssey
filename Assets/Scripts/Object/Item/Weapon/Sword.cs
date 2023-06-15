using System.Collections;
using UnityEngine;

public class Sword : Weapon
{
    private Coroutine checkAttackReInputCor;

    public override void Attack()
    {
        ComboCount++;
        Player.Instance.animator.SetFloat(AttackState.hashAttackSpeedAnimation, Player.Instance.Status.AttackSpeed);
        Player.Instance.animator.SetBool(AttackState.hashIsAttackAnimation, true);
        Player.Instance.animator.SetInteger(AttackState.hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
    }

    public override void ChargingAttack()
    {

    }

    public override void DashAttack()
    {

    }

    public override void Skill()
    {

    }

    public override void UltimateSkill()
    {

    }

    public void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            StopCoroutine(checkAttackReInputCor);
        checkAttackReInputCor = StartCoroutine(CheckAttackReInputCoroutine(reInputTime));
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
        Player.Instance.animator.SetInteger(AttackState.hashAttackAnimation, 0);
    }

    public override void Use()
    {
        Attack();
    }

    public override void ApplyStatusModifier()
    {
        Player.Instance.Status.AttackPower += weaponData.AttackPower;
        Player.Instance.Status.AttackSpeed = weaponData.AttackSpeed;
    }

    public override void RemoveStatusModifier()
    {
        Player.Instance.Status.AttackPower -= weaponData.AttackPower;
        Player.Instance.Status.AttackSpeed = 1;
    }
}