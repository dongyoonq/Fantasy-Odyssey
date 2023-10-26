using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sword : Weapon
{
    List<Player.Input> commandKey;

    Coroutine bufferTimer;
    ParticleSystem particle;
    Queue<Player.Input> commandBuffer;

    Vector3 playerRot;
    Vector3 playerPos;
    Vector3 playerHandPos;

    WaitForSeconds[] cacheYield;

    private void Awake()
    {
        cacheYield = new WaitForSeconds[11];

        for (int i = 1; i < cacheYield.Length; i++)
        {
            cacheYield[i] = new WaitForSeconds(i * 0.1f);
        }
    }

    private void OnEnable()
    {
        equipmentData = Resources.Load<EquipmentData>("Data/ItemData/WeaponData/NormalSword");
        weaponData = Resources.Load<WeaponData>("Data/ItemData/WeaponData/NormalSword");
        commandBuffer = new Queue<Player.Input>();
        commandKey = new List<Player.Input>()
        {
            Player.Input.LAttack,
            Player.Input.LAttack,
            Player.Input.RAttack,
            Player.Input.LAttack
        };
    }

    public Sword(WeaponData data) : base(data)
    {
    }

    private void Start()
    {

    }

    private void LateUpdate()
    {
        playerRot = Player.Instance.transform.rotation.eulerAngles;
        playerPos = Player.Instance.transform.position;
        playerHandPos = Player.Instance.hand.position;
    }

    public override void LeftAttack()
    {
        if (ComboCount == 1)
            FirstAttack();
        else if (ComboCount == 2)
            SecondAttack();
        else if (ComboCount == 3)
            FinishAttack();

        if (ComboCount >= weaponData.MaxCombo)
            StopCommandBufferTimer();
    }

    void FirstAttack()
    {
        GameManager.Sound.PlaySFX("NormalSword1");
        TotalDamage = Player.Instance.Status.AttackPower;
        AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
        particle = GameManager.Resource.Instantiate(weaponData.Effects[0],
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y, -160), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.4f);
    }

    void SecondAttack()
    {
        GameManager.Sound.PlaySFX("NormalSword2");
        TotalDamage = Player.Instance.Status.AttackPower;
        AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
        particle =
            GameManager.Resource.Instantiate(weaponData.Effects[0],
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y - 25f, 15), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.4f);
    }

    void FinishAttack()
    {
        GameManager.Sound.PlaySFX("NormalSword3");
        TotalDamage = Player.Instance.Status.AttackPower + 30;
        AttackCircleJudgement(TotalDamage, 2.5f, 360f, 240f);
        particle = GameManager.Resource.Instantiate(weaponData.Effects[1],
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y - 60f, 0), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.7f);
    }

    public override void RightAttack()
    {
        TotalDamage = Player.Instance.Status.AttackPower - 20;
        AttackBoxJudgement(TotalDamage, transform.position + transform.forward * 1f, new Vector3(0.4f, 0.4f, 1.8f),
            Quaternion.Euler(Player.Instance.transform.rotation.eulerAngles));
        StartCoroutine(syncRightAttackParticle());
    }

    IEnumerator syncRightAttackParticle()
    {
        yield return cacheYield[3];
        GameManager.Sound.PlaySFX("NormalSword5");
        particle = GameManager.Resource.Instantiate(weaponData.Effects[2],
            new Vector3(playerHandPos.x, playerHandPos.y - 0.2f, playerHandPos.z), Quaternion.Euler(playerRot), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.4f);
    }

    public override void ChargingAttack()
    {
        StartCoroutine(ActiveChargeAttackHitBox());
        StartCoroutine(syncChargeAttackParticle());
        StopCommandBufferTimer();
    }

    IEnumerator syncChargeAttackParticle()
    {
        yield return cacheYield[4];
        GameManager.Sound.PlaySFX("NormalSword4");
        particle = GameManager.Resource.Instantiate(weaponData.Effects[4],
            playerPos + (Player.Instance.transform.forward * 3.5f), Quaternion.Euler(playerRot), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.8f);
        yield return cacheYield[6];
        particle = GameManager.Resource.Instantiate(weaponData.Effects[5],
            new Vector3(playerPos.x, 0, playerPos.z) + Player.Instance.transform.forward * 5f, Quaternion.identity, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.5f);
    }

    IEnumerator ActiveChargeAttackHitBox()
    {
        yield return cacheYield[7];
        transform.GetChild(0).gameObject.SetActive(true);
        yield return cacheYield[5];
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void DashAttack()
    {
        StartCoroutine(ActiveDashAttackHitBox());
        StartCoroutine(syncDashttackParticle());
        StopCommandBufferTimer();
    }

    IEnumerator syncDashttackParticle()
    {
        yield return cacheYield[9];
        GameManager.Sound.PlaySFX("NormalSword4");
        particle = GameManager.Resource.Instantiate(weaponData.Effects[0],
            playerPos, Quaternion.Euler(playerRot.x, playerRot.y, playerRot.z - 90), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.3f);
    }

    IEnumerator ActiveDashAttackHitBox()
    {
        yield return cacheYield[9];
        transform.GetChild(1).gameObject.SetActive(true);
        yield return cacheYield[3];
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void Skill()
    {
        StartCoroutine(ActiveSkillAttackHitBox());
        StartCoroutine(syncSkillParticle());
    }

    IEnumerator syncSkillParticle()
    {
        yield return cacheYield[3];
        GameManager.Sound.PlaySFX("NormalSword6");
        particle = GameManager.Resource.Instantiate(weaponData.Effects[3],
            playerHandPos, Quaternion.Euler(playerRot), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.8f);
    }

    IEnumerator ActiveSkillAttackHitBox()
    {
        GameObject hitObj = GameManager.Resource.Instantiate<GameObject>("Prefabs/Item/Weapon/NormalSword/SkillAttackHitBox", true);

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

        GameManager.Resource.Destroy(hitObj);
    }

    public override void UltimateSkill()
    {
        StartCoroutine(syncUltParticle());
        StopCommandBufferTimer();
    }

    IEnumerator syncUltParticle()
    {
        TotalDamage = Player.Instance.Status.AttackPower;
        yield return cacheYield[1];
        GameManager.Sound.PlaySFX("NormalSword1");
        AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
        particle = GameManager.Resource.Instantiate(weaponData.Effects[0],
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y, -140), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.5f);
        yield return cacheYield[5];
        GameManager.Sound.PlaySFX("NormalSword2");
        AttackCircleJudgement(TotalDamage, 2.2f, 190f, 240f);
        particle = GameManager.Resource.Instantiate(weaponData.Effects[0],
            new Vector3(playerHandPos.x - 0.5f, playerHandPos.y, playerHandPos.z), Quaternion.Euler(playerRot.x, playerRot.y, 40), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.5f);
        yield return cacheYield[5];
        GameManager.Sound.PlaySFX("NormalSword3");
        AttackCircleJudgement(TotalDamage + 20, 2.5f, 190f, 240f);
        particle = GameManager.Resource.Instantiate(weaponData.Effects[0],
            playerHandPos, Quaternion.Euler(playerRot.x, playerRot.y, 140), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.5f);
        yield return cacheYield[5];
        GameManager.Sound.PlaySFX("NormalSword5");
        AttackBoxJudgement(TotalDamage + 30, transform.position + transform.forward * 1f, new Vector3(0.4f, 0.4f, 1.8f),
            Quaternion.Euler(Player.Instance.transform.rotation.eulerAngles));
        particle = GameManager.Resource.Instantiate(weaponData.Effects[2],
            playerHandPos, Quaternion.Euler(new Vector3(playerRot.x + 25f, playerRot.y, playerRot.z)), Player.Instance.transform, true);
        GameManager.Resource.Destroy(particle.gameObject, 0.2f);
    }

    public override void Attack()
    {
        if (Player.Instance.playerController.isCharging)
        {
            Player.Instance.stateMachine.ChangeState(StateName.ChargeAttack);
            return;
        }
        else if (Player.Instance.playerController.isUltAttack)
        {
            Player.Instance.stateMachine.ChangeState(StateName.UltAttack);
            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.RAttack)
        {
            commandBuffer.Enqueue(Player.Instance.inputBuffer.Dequeue());

            if (commandBuffer.Count >= commandKey.Count)
                CheckCommandSkill();

            Player.Instance.stateMachine.ChangeState(StateName.RAttack);
            return;
        }
        else if (Player.Instance.inputBuffer.Count != 0 && Player.Instance.inputBuffer.Peek() == Player.Input.Dash)
        {
            Player.Instance.inputBuffer.Clear();
            Player.Instance.stateMachine.ChangeState(StateName.DashAttack);
            return;
        }

        if (bufferTimer == null && gameObject.IsValid())
            bufferTimer = StartCoroutine(CommandTimer());

        commandBuffer.Enqueue(Player.Instance.inputBuffer.Dequeue());

        if (commandBuffer.Count >= commandKey.Count)
        {
            if (CheckCommandSkill())
            {
                Player.Instance.stateMachine.ChangeState(StateName.SkillAttack);
                StopCommandBufferTimer();
                return;
            }
        }

        Player.Instance.stateMachine.ChangeState(StateName.LAttack);
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
            if (commandKey[i] != commandBuffer.Dequeue())
                return false;

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