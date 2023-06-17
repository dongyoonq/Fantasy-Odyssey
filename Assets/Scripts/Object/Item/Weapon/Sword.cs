using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sword : Weapon
{
    List<Player.Input> commandKey = new List<Player.Input>()
    {   Player.Input.LAttack,
        Player.Input.LAttack,
        Player.Input.RAttack,
        Player.Input.LAttack
    };

    AttackState attackState;
    Coroutine bufferTimer;
    Queue<Player.Input> commandBuffer = new Queue<Player.Input>();

    private Coroutine checkAttackReInputCor;
    public readonly int hashIsRightAttack = Animator.StringToHash("IsRightAttack");
    public readonly int hashIsDashAttack = Animator.StringToHash("IsDashAttack");
    public readonly int hashIsChargingAttack = Animator.StringToHash("IsChargingAttack");
    public readonly int hashIsSkillAtaack = Animator.StringToHash("IsSkillAttack");
    public readonly int hashIsUltSkillAtaack = Animator.StringToHash("IsUltAttack");

    private void Start()
    {
        attackState = Player.Instance.playerController.attackState;
    }

    public override void LeftAttack()
    {
        Player.Instance.animator.SetFloat(attackState.hashAttackSpeedAnimation, Player.Instance.Status.AttackSpeed);
        Player.Instance.animator.SetBool(attackState.hashIsLeftAttackAnimation, true);
        Player.Instance.animator.SetInteger(attackState.hashAttackAnimation, ++ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);

        if (ComboCount >= weaponData.MaxCombo)
            StopCommandBufferTimer();
    }

    public void RightAttack()
    {
        Player.Instance.animator.SetBool(hashIsRightAttack, true);
        ComboCount = 0;
    }

    public override void ChargingAttack()
    {
        Player.Instance.animator.SetBool(hashIsChargingAttack, true);
        ComboCount = 0;
    }

    public override void DashAttack()
    {
        Player.Instance.animator.SetBool(hashIsDashAttack, true);
        ComboCount = 0;
    }

    public override void Skill()
    {
        Player.Instance.animator.SetBool(hashIsSkillAtaack, true);
        ComboCount = 0;
    }

    public override void UltimateSkill()
    {
        Player.Instance.animator.SetBool(hashIsUltSkillAtaack, true);
        Player.Instance.playerController.isUltAttack = false;
        ComboCount = 0;
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
        Player.Instance.animator.SetInteger(attackState.hashAttackAnimation, 0);
    }

    public override void Use()
    {
        if (Player.Instance.playerController.isCharging)
        {
            StopCommandBufferTimer();
            ChargingAttack();

            return;
        }
        else if (Player.Instance.playerController.isUltAttack)
        {
            StopCommandBufferTimer();
            UltimateSkill();

            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.RAttack)
        {
            commandBuffer.Enqueue(Player.Instance.inputBuffer.Peek());
            Player.Instance.inputBuffer.Dequeue();

            if (commandBuffer.Count >= commandKey.Count)
                CheckCommandSkill();

            RightAttack();
            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
        {
            Player.Instance.inputBuffer.Clear();
            Player.Instance.animator.SetBool("Dash", false);

            StopCommandBufferTimer();
            DashAttack();

            return;
        }

        if (bufferTimer == null)
            bufferTimer = StartCoroutine(CommandTimer());

        commandBuffer.Enqueue(Player.Instance.inputBuffer.Peek());
        Player.Instance.inputBuffer.Dequeue();

        if (commandBuffer.Count >= commandKey.Count)
        {
            if (CheckCommandSkill())
            {
                Skill();
                StopCommandBufferTimer();
                return;
            }
        }

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

    private bool CheckCommandSkill()
    {
        for (int i = 0; i < commandKey.Count; i++)
        {
            if (commandKey[i] != commandBuffer.Dequeue())
                return false;
        }

        commandBuffer.Clear();
        return true;
    }

    private IEnumerator CommandTimer()
    {
        float currentTime = 0f;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 4f)
                break;


            yield return null;
        }

        StopCommandBufferTimer();
    }

    void StopCommandBufferTimer()
    {
        //Debug.Log("타이머 종료");
        if (bufferTimer != null)
        {
            StopCoroutine(bufferTimer);
            bufferTimer = null;
        }

        commandBuffer.Clear();
    }
}