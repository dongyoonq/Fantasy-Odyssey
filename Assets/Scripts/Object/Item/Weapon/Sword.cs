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
    ParticleSystem particle;
    Queue<Player.Input> commandBuffer = new Queue<Player.Input>();

    private Coroutine checkAttackReInputCor;
    public readonly int hashIsRightAttack = Animator.StringToHash("IsRightAttack");
    public readonly int hashIsDashAttack = Animator.StringToHash("IsDashAttack");
    public readonly int hashIsChargingAttack = Animator.StringToHash("IsChargingAttack");
    public readonly int hashIsSkillAtaack = Animator.StringToHash("IsSkillAttack");
    public readonly int hashIsUltSkillAtaack = Animator.StringToHash("IsUltAttack");

    Vector3 playerRot;
    Vector3 playerPos;
    Vector3 playerHandPos;

    private void Start()
    {
        attackState = Player.Instance.playerController.attackState;
    }

    private void LateUpdate()
    {
        playerRot = Player.Instance.transform.rotation.eulerAngles;
        playerPos = Player.Instance.transform.position;
        playerHandPos = Player.Instance.hand.position;
    }

    public override void LeftAttack()
    {
        if (ComboCount == 0)
        {
            TotalDamage = Player.Instance.Status.AttackPower;
            AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
            particle = GameManager.Resouce.Instantiate(weaponData.Effects[0],
                playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y, -160), Player.Instance.transform, true);
            Destroy(particle.gameObject, 0.4f);
        }
        else if (ComboCount == 1)
        {
            TotalDamage = Player.Instance.Status.AttackPower;
            AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
            particle = 
                GameManager.Resouce.Instantiate(weaponData.Effects[0], 
                playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y - 25f, 15), Player.Instance.transform, true);
            Destroy(particle.gameObject, 0.4f);
        }
        else if (ComboCount == 2)
        {
            TotalDamage = Player.Instance.Status.AttackPower + 30;
            AttackCircleJudgement(TotalDamage, 2.5f, 360f, 240f);
            particle = GameManager.Resouce.Instantiate(weaponData.Effects[1], 
                playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y - 60f, 0), Player.Instance.transform, true);
            Destroy(particle.gameObject, 0.7f);
        }

        Player.Instance.animator.SetFloat(attackState.hashAttackSpeedAnimation, Player.Instance.Status.AttackSpeed);
        Player.Instance.animator.SetBool(attackState.hashIsLeftAttackAnimation, true);
        Player.Instance.animator.SetInteger(attackState.hashAttackAnimation, ++ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);

        if (ComboCount >= weaponData.MaxCombo)
            StopCommandBufferTimer();
    }

    public void RightAttack()
    {
        TotalDamage = Player.Instance.Status.AttackPower - 20;
        AttackBoxJudgement(TotalDamage, transform.position + transform.forward * 1f, new Vector3(0.4f, 0.4f, 1.8f), 
            Quaternion.Euler(Player.Instance.transform.rotation.eulerAngles));
        StartCoroutine(syncRightAttackParticle());
        Player.Instance.animator.SetBool(hashIsRightAttack, true);
        ComboCount = 0;
    }

    IEnumerator syncRightAttackParticle()
    {
        yield return new WaitForSeconds(0.3f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[2], 
            new Vector3(playerHandPos.x, playerHandPos.y - 0.2f, playerHandPos.z), Quaternion.Euler(playerRot), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.4f);
    }

    public override void ChargingAttack()
    {
        StartCoroutine(ActiveChargeAttackHitBox());
        StartCoroutine(syncChargeAttackParticle());
        Player.Instance.animator.SetBool(hashIsChargingAttack, true);
        ComboCount = 0;
    }

    IEnumerator syncChargeAttackParticle()
    {
        yield return new WaitForSeconds(0.4f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[4], 
            playerPos + (Player.Instance.transform.forward * 3.5f), Quaternion.Euler(playerRot), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.8f);
        yield return new WaitForSeconds(0.6f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[5], 
            new Vector3(playerPos.x, 0, playerPos.z) + Player.Instance.transform.forward * 5f, Quaternion.identity, true);
        Destroy(particle.gameObject, 0.5f);
    }

    IEnumerator ActiveChargeAttackHitBox()
    {
        yield return new WaitForSeconds(0.7f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void DashAttack()
    {
        StartCoroutine(ActiveDashAttackHitBox());
        StartCoroutine(syncDashttackParticle());
        Player.Instance.animator.SetBool(hashIsDashAttack, true);
        ComboCount = 0;
    }

    IEnumerator syncDashttackParticle()
    {
        yield return new WaitForSeconds(0.9f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[0], 
            playerPos, Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z - 90), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.3f);
    }

    IEnumerator ActiveDashAttackHitBox()
    {
        yield return new WaitForSeconds(0.9f);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void Skill()
    {
        StartCoroutine(ActiveSkillAttackHitBox());
        StartCoroutine(syncSkillParticle());
        Player.Instance.animator.SetBool(hashIsSkillAtaack, true);
        ComboCount = 0;
    }

    IEnumerator syncSkillParticle()
    {
        yield return new WaitForSeconds(0.3f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[3], 
            playerHandPos, Quaternion.Euler(playerRot), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.8f);
    }

    IEnumerator ActiveSkillAttackHitBox()
    {
        GameObject hitObj = GameManager.Resouce.Instantiate<GameObject>("Prefabs/Sword/SkillAttackHitBox", true);

        float rate = 0;
        Vector3 start = Player.Instance.transform.position;
        Vector3 end = Player.Instance.transform.position + (Player.Instance.transform.forward * 10f) + (Player.Instance.transform.up * 1f);
        float totalTime = Vector3.Distance(start, end) / 10f;

        while (rate < totalTime)
        {
            rate += Time.deltaTime;
            hitObj.transform.position = Vector3.Lerp(start, end, rate);
            yield return null;
        }

        Destroy(hitObj);
    }

    public override void UltimateSkill()
    {
        StartCoroutine(syncUltParticle());
        Player.Instance.animator.SetBool(hashIsUltSkillAtaack, true);
        Player.Instance.playerController.isUltAttack = false;
        ComboCount = 0;
    }

    IEnumerator syncUltParticle()
    {
        TotalDamage = Player.Instance.Status.AttackPower;
        yield return new WaitForSeconds(0.1f);
        AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[0],
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y, -140), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.5f);
        yield return new WaitForSeconds(0.5f);
        AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[0], 
            new Vector3(playerHandPos.x - 0.5f, playerHandPos.y, playerHandPos.z), Quaternion.Euler(playerRot.x, playerRot.y, 40), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.5f);
        yield return new WaitForSeconds(0.5f);
        AttackCircleJudgement(TotalDamage + 20, 2.5f, 190f, 240f);
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[0], 
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y, 140), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.5f);
        yield return new WaitForSeconds(0.5f);
        AttackBoxJudgement(TotalDamage + 30, transform.position + transform.forward * 1f, new Vector3(0.4f, 0.4f, 1.8f),
            Quaternion.Euler(Player.Instance.transform.rotation.eulerAngles));
        particle = GameManager.Resouce.Instantiate(weaponData.Effects[2], 
            playerHandPos, Quaternion.Euler(new Vector3(playerRot.x + 25f, playerRot.y, playerRot.z)), Player.Instance.transform, true);
        Destroy(particle.gameObject, 0.2f);
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