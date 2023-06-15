using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private Coroutine checkAttackReInputCor;
    public static readonly int hashIsDashAttack = Animator.StringToHash("IsDashAttack");
    public static readonly int hashIsChargingAttack = Animator.StringToHash("IsChargingAttack");
    public static readonly int hashIsRightAttack = Animator.StringToHash("IsRightAttack");

    public override void LeftAttack()
    {
        ComboCount++;
        Player.Instance.animator.SetFloat(AttackState.hashAttackSpeedAnimation, Player.Instance.Status.AttackSpeed);
        Player.Instance.animator.SetBool(AttackState.hashIsLeftAttackAnimation, true);
        Player.Instance.animator.SetInteger(AttackState.hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
    }

    public void RightAttack()
    {
        Player.Instance.animator.SetBool(hashIsRightAttack, true);
        ComboCount = 0;
    }

    public override void ChargingAttack()
    {
        Player.Instance.animator.applyRootMotion = false;
        Player.Instance.animator.SetBool(hashIsChargingAttack, true);
        ComboCount = 0;
    }

    public override void DashAttack()
    {
        Player.Instance.animator.applyRootMotion = true;
        Player.Instance.animator.SetBool(hashIsDashAttack, true);
        ComboCount = 0;
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

    Queue<Player.Input> commandBuffer = new Queue<Player.Input>();

    public override void Use()
    {
        if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.RAttack)
        {
            commandBuffer.Enqueue(Player.Instance.inputBuffer.Peek());
            if (commandBuffer.Count >= 4)
                Debug.Log("스킬 발동..");
            RightAttack();
            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
        {
            while(Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
            {
                Player.Instance.inputBuffer.Dequeue();
            }

            Player.Instance.animator.SetBool("Dash", false);
            DashAttack();
            return;
        }
        else if (PlayerController.isCharging)
        {
            ChargingAttack();
            return;
        }

        commandBuffer.Enqueue(Player.Instance.inputBuffer.Peek());
        if (commandBuffer.Count >= 4)
            Debug.Log("스킬 발동..");
        LeftAttack();
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