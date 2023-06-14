using System.Collections;
using UnityEngine;

public class TestWeapon : Weapon
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    private Coroutine checkAttackReInputCor;

    public override void Attack()
    {
        ComboCount++;
        Player.Instance.animator.SetFloat(hashAttackSpeedAnimation, WeaponData.AttackSpeed);
        Player.Instance.animator.SetBool(hashIsAttackAnimation, true);
        Player.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
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
        Player.Instance.animator.SetInteger(hashAttackAnimation, 0);
    }

    public override void Use()
    {
        Attack();
    }

    public override void ApplyStatusModifier(Player player)
    {

    }

    public override void RemoveStatusModifier(Player player)
    {

    }
}