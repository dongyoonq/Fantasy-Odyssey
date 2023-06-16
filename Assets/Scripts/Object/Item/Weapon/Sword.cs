using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sword : Weapon
{
    private Coroutine checkAttackReInputCor;
    public readonly int hashIsRightAttack = Animator.StringToHash("IsRightAttack");
    public readonly int hashIsDashAttack = Animator.StringToHash("IsDashAttack");
    public readonly int hashIsChargingAttack = Animator.StringToHash("IsChargingAttack");
    public readonly int hashIsSkillAtaack = Animator.StringToHash("IsSkillAttack");
    public readonly int hashIsUltSkillAtaack = Animator.StringToHash("IsUltAttack");

    List<Player.Input> commandKey = new List<Player.Input>() { Player.Input.LAttack, Player.Input.LAttack, Player.Input.RAttack, Player.Input.LAttack };

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

    public override void UltimateSkill(PlayerController controller)
    {
        Player.Instance.animator.SetBool(hashIsUltSkillAtaack, true);
        controller.isUltAttack = false;
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
        Player.Instance.animator.SetInteger(AttackState.hashAttackAnimation, 0);
    }

    Queue<Player.Input> commandBuffer = new Queue<Player.Input>();

    public override void Use(PlayerController controller)
    {
        if (controller.isCharging)
        {
            if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
                Player.Instance.inputBuffer.Dequeue();

            if (bufferTimer != null)
            {
                StopCoroutine(bufferTimer);
                bufferTimer = null;
            }

            commandBuffer.Clear();
            ChargingAttack();

            return;
        }
        else if (controller.isUltAttack) 
        {
            if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
                Player.Instance.inputBuffer.Dequeue();

            if (bufferTimer != null)
            {
                StopCoroutine(bufferTimer);
                bufferTimer = null;
            }

            commandBuffer.Clear();
            UltimateSkill(controller);

            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.RAttack)
        {
            commandBuffer.Enqueue(Player.Instance.inputBuffer.Peek());
            Debug.Log(Player.Instance.inputBuffer.Peek());
            Player.Instance.inputBuffer.Dequeue();

            while (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
                Player.Instance.inputBuffer.Dequeue();

            if (commandBuffer.Count >= commandKey.Count)
                CheckCommandSkill();

            RightAttack();
            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
        {
            while (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
                Player.Instance.inputBuffer.Dequeue();

            if (Player.Instance.inputBuffer.Count != 0)
                Player.Instance.inputBuffer.Dequeue();

            Player.Instance.animator.SetBool("Dash", false);

            if (bufferTimer != null)
            {
                StopCoroutine(bufferTimer);
                bufferTimer = null;
            }

            commandBuffer.Clear();
            DashAttack();

            return;
        }

        if (bufferTimer == null)
            bufferTimer = StartCoroutine(CommandTimer());

        while (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
            Player.Instance.inputBuffer.Dequeue();

        commandBuffer.Enqueue(Player.Instance.inputBuffer.Peek());
        Debug.Log(Player.Instance.inputBuffer.Peek());
        Player.Instance.inputBuffer.Dequeue();

        if (commandBuffer.Count >= commandKey.Count)
        {
            if (CheckCommandSkill())
            {
                Skill();

                if (bufferTimer != null)
                {
                    StopCoroutine(bufferTimer);
                    bufferTimer = null;
                }
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

    Coroutine bufferTimer;

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

        commandBuffer.Clear();
        Debug.Log("타이머 종료");

        if (bufferTimer != null)
        {
            StopCoroutine(bufferTimer);
            bufferTimer = null;
        }
    }
}