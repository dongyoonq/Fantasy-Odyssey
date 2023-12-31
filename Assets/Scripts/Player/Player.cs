using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IHitable
{
    public UnityEvent<ItemData> OnChangeShortSlot;
    public UnityEvent<BaseMonsterData> OnChangeKillQuestUpdate;
    public UnityEvent<ItemData> OnChangeGatheringQuestUpdate;
    public UnityEvent<ItemData> OnChangeUseQuestUpdate;
    public UnityEvent<NpcData> OnChangeTalkQuestUpdate;
    public UnityEvent OnChangedHp;
    public UnityEvent<ItemData, int, int> OnAddItemInventory;
    public UnityEvent<ItemData, int, int> OnRemoveItemInventory;
    public UnityEvent OnLevelUp;
    public UnityEvent OnChangeEquipment;
    public UnityEvent OnDied;

    public static Player Instance { get { return instance; } }
    private static Player instance;

    public PlayerController playerController { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public CharacterController controller { get; private set; }
    public MouseController mouseController { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public Inventory inventory { get; private set; }
    public InventoryUI inventoryUI { get; set; }
    public QuestUI questUI { get; set; }
    public EquipmentUI equipUI { get; set; }
    public MonsterInfoUI monsterUI { get; set; }
    public ShortSlotUI shortUI { get; set; }
    public StatusUI statusUI { get; set; }
    public NoticedUI noticeUI { get; set; }
    public List<Quest> questList { get; private set; }
    public List<Quest> completeQuest { get; private set; }

    // public Dictionary<EquipmentData.EquipType, Equipment> wearingEquip { get; private set; }
    public Equipment[] wearingEquip = new Equipment[(int)EquipmentData.EquipType.Size];
    public Queue<Input> inputBuffer { get; private set; }

    [SerializeField] PlayerStatusData status;
    public PlayerStatusData Status { get { return status; } set { status = value; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    // * OnChangedHp 이벤트 발생시키지 않을때만 사용
    [SerializeField] public int currentHp; 
    [SerializeField] int level;
    [SerializeField] string playerName;

    public int exp;
    public int nextLevelExp;

    RuntimeAnimatorController defaultAnimator;

    public int CurrentHP { 
        get { return currentHp; } 
        set 
        {
            if (value - currentHp < 0) 
                OnChangedHp?.Invoke();

            currentHp = value;

            if (currentHp <= 0)
            {
                StopAllCoroutines();
                currentHp = 0;
                OnDied?.Invoke();
                stateMachine.ChangeState(StateName.Die);
            }
        }
    }

    public int Level { get { return level; } set { level = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int NextLevelExp { get { return nextLevelExp; } }

    [NonSerialized] public float MoveSpeed;
    [NonSerialized] public float YSpeed;

    [SerializeField] public Transform hand;
    [SerializeField] public Transform foot;

    public enum Input
    {
        LAttack,
        RAttack,
        Dash,
        Jump,
    }

    void Awake()
    {
        MoveSpeed = status.WalkSpeed;

        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        questList = new List<Quest>();
        completeQuest = new List<Quest>();
        inputBuffer = new Queue<Input>();
        controller = GetComponent<CharacterController>();
        mouseController = GetComponent<MouseController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        defaultAnimator = GetComponent<Animator>().runtimeAnimatorController;
        playerController = GetComponent<PlayerController>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        SetStatus();
        DontDestroyOnLoad(gameObject);
        InitStateMachine();
    }

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        OnLevelUp.AddListener(() =>
        {
            LevelUp();
            LevelUpEffect();
        });
        OnDied.AddListener(() => GameManager.Sound.PlayMusic("Die"));
    }

    void Update()
    {
        stateMachine?.UpdateState();

        if (exp >= nextLevelExp)
            OnLevelUp?.Invoke();

        ScanNpc();
        ScanMonster();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitStateMachine()
    {
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller)); // 등록
        stateMachine.AddState(StateName.Dash, new DashState(controller));
        stateMachine.AddState(StateName.ATTACK, new BaseAttackState(controller));
        stateMachine.AddState(StateName.LAttack, new LeftAttackState(controller));
        stateMachine.AddState(StateName.RAttack, new RightAttackState(controller));
        stateMachine.AddState(StateName.ChargeAttack, new ChargeAttackState(controller));
        stateMachine.AddState(StateName.SkillAttack, new SkillAttackState(controller));
        stateMachine.AddState(StateName.UltAttack, new UltAttackState(controller));
        stateMachine.AddState(StateName.DashAttack, new DashAttackState(controller));
        stateMachine.AddState(StateName.Die, new DieState(controller));
    }

    void SetStatus()
    {
        currentHp = Status.MaxHp;
        level = 1;
        exp = 0;
        nextLevelExp = 1000;
        Status.AttackPower = Status.attackPower;
        Status.AttackSpeed = Status.attackSpeed;
        Status.Deffense = Status.deffense;
        Status.MaxHp = Status.maxHP;
        Status.WalkSpeed = Status.walkSpeed;
        Status.RunSpeed = Status.runSpeed;
        Status.JumpPower = Status.jumpPower;
    }

    // 인벤토리 추가 메서드
    public void AddItemToInventory(ItemData item)
    {
        if (inventory.list.Count(x => x == null) == 0)
        {
            GameManager.Ui.SetFloating(gameObject, "가방이 가득찼다", new Color(1, 0, 0, 1), 0.1f, 5f);
            return;
        }

        int index = -1;
        if (item is CountableItemData)
        {
            index = inventory.list.FindIndex(x => x == item);

            if (index == -1)
            {
                for (int i = 0; i < inventory.list.Count; i++)
                {
                    if (inventory.list[i] == null)
                    {
                        inventory.list[i] = item;
                        inventoryUI.slots[i].slotIndex = i;
                        index = i;
                        break;
                    }
                }
            }

            OnChangeGatheringQuestUpdate?.Invoke(item);
            OnChangeShortSlot?.Invoke(item);
            OnAddItemInventory?.Invoke(item, index, 1);
            GameManager.Ui.SetFloating(gameObject, $"+{item.itemName}", new Color(0, 1, 0, 1), 0.1f, 5f);
            return;
        }

        for (int i = 0; i < inventory.list.Count; i++)
        {
            if (inventory.list[i] == null)
            {
                inventory.list[i] = item;
                inventoryUI.slots[i].slotIndex = i;
                index = i;
                break;
            }
        }

        OnChangeGatheringQuestUpdate?.Invoke(item);
        OnChangeShortSlot?.Invoke(item);
        OnAddItemInventory?.Invoke(item, index, 1);
        GameManager.Ui.SetFloating(gameObject, $"+{item.itemName}", new Color(0, 1, 0, 1), 0.1f, 5f);
    }

    public void AddItemToInventory(ItemData item, int index = 0)
    {
        OnAddItemInventory?.Invoke(item, index, 1);
        GameManager.Ui.SetFloating(gameObject, $"+{item.itemName}", new Color(0, 1, 0, 1), 0.1f, 5f);
    }

    // 인벤토리 아이템 제거 메서드
    public void RemoveItemFromInventory(ItemData item, int index = -1)
    {
        if (index == -1)
        {
            for (int i = 0; i < inventory.list.Count; i++)
            {
                if (inventory.list[i] == item)
                {
                    index = i;
                    break;
                }
            }
        }

        OnRemoveItemInventory?.Invoke(item, index, 1);

        if (item is not CountableItemData)
        {
            inventory.list[index] = null;
        }
        else if (item is CountableItemData)
        {
            if (inventoryUI.slots[index].amount == 0)
                inventory.list[index] = null;
        }

        OnChangeShortSlot?.Invoke(item);
        inventory.onChangeInventory?.Invoke();
        StartCoroutine(GameManager.Ui.SetFloatingDelay(gameObject, $"-{item.itemName}", new Color(1, 0, 0, 1), 0.8f, 5f, 0.2f));
    }

    /// <summary>
    /// 장비 착용 이벤트 핸들러
    /// </summary>
    /// <param questName="sender"></param>
    /// <param questName="equipment"></param>   
    public bool OnEquip(Equipment equipment, int index = -1)
    {
        // 인벤토리에 장비를 지우고
        RemoveItemFromInventory(equipment.Data, index);

        // 들어온 아이템이 그 부위에 착용중이면 장비를 벗는다.
        if (wearingEquip[(int)equipment.equipmentData.equipType] != null)
        {
            UnEquip(equipment, index);
        }

        // 착용한 분위에 이 장비를 착용 시킨다.
        wearingEquip[(int)equipment.equipmentData.equipType] = equipment;

        // 그 장비에 대한 스텟 적용
        equipment.ApplyStatusModifier();

        if (equipment.equipmentData.equipType == EquipmentData.EquipType.Weapon)
            RegisterWeapon(equipment.gameObject);

        OnChangeEquipment?.Invoke();
        return true;
    }

    /// <summary>
    /// 장비 착용해제 이벤트 핸들러
    /// </summary>
    /// <param questName="sender"></param>
    /// <param questName="equipment"></param>
    public bool UnEquip(Equipment equipment, int index = -1)
    {
        // 들어온 아이템이없으면 빠져나온다.
        if (equipment == null)
            return false;

        // 착용중인 부위에 아이템이 있으면
        if (wearingEquip[(int)equipment.equipmentData.equipType] != null)
        {
            // 임시 착용중인 장비
            Equipment temp = wearingEquip[(int)equipment.equipmentData.equipType];

            // 스텟 미적용
            temp.RemoveStatusModifier();

            if (equipment.equipmentData.equipType == EquipmentData.EquipType.Weapon)
                UnRegisterWeapon(wearingEquip[(int)equipment.equipmentData.equipType] as Weapon);

            // 착용중인 장비를 지워준다.
            Destroy(wearingEquip[(int)equipment.equipmentData.equipType].gameObject);
            wearingEquip[(int)equipment.equipmentData.equipType] = null;

            inventory.list[index] = temp.equipmentData;
            AddItemToInventory(temp.equipmentData, index);

            OnChangeEquipment?.Invoke();

            return true;

            /*
            // 착용한 분위에 이 장비를 착용 시킨다.
            wearingEquip.Add(equipment.equipmentData.equipType, equipment);

            // 인벤토리에 장비를 지우고
            RemoveItemFromInventory(inventory.list[index], index);

            // 인벤토리에 착용 중인 장비를 넣어주고
            inventory.list[index] = temp;
            AddItemToInventory(temp, index);

            OnChangeEquipment?.Invoke();
            return true;*/
        }
        else
        {
            return false;
        }
    }

    public bool UnEquip(Equipment equipment)
    {
        if (equipment == null)
            return false;

        if (inventory.list.Count(x => x == null) == 0)
        {
            Debug.Log("가방이 가득찼다");
            return false;
        }

        int index = -1;

        for (int i = 0; i < inventory.list.Count; i++)
        {
            if (inventory.list[i] == null)
            {
                index = i;
                break;
            }
        }

        Equipment temp = wearingEquip[(int)equipment.equipmentData.equipType];

        // 스텟 미적용
        temp.RemoveStatusModifier();

        if (equipment.equipmentData.equipType == EquipmentData.EquipType.Weapon)
            UnRegisterWeapon(wearingEquip[(int)equipment.equipmentData.equipType] as Weapon);

        // 착용중인 장비를 지워준다.
        Destroy(wearingEquip[(int)equipment.equipmentData.equipType].gameObject);
        wearingEquip[(int)equipment.equipmentData.equipType] = null;

        inventory.list[index] = temp.equipmentData;
        AddItemToInventory(temp.equipmentData, index);

        OnChangeEquipment?.Invoke();
        return true;
    }

    // 무기 등록
    public void RegisterWeapon(GameObject weapon)
    {
        if (wearingEquip[(int)EquipmentData.EquipType.Weapon] != null)
        {
            Weapon weaponInfo = weapon.GetComponent<Weapon>();
            weapon.transform.SetParent(hand);
            weapon.transform.localPosition = weaponInfo.weaponData.localPosition;
            weapon.transform.localEulerAngles = weaponInfo.weaponData.localRotation;
            weapon.transform.localScale = weaponInfo.weaponData.localScale;
            animator.runtimeAnimatorController = weaponInfo.weaponData.WeaponAnimator;
            weapon.gameObject.SetActive(true);
        }
    }

    // 무기 삭제
    public void UnRegisterWeapon(Weapon weapon)
    {
        if (weapon.equipmentData.equipType == EquipmentData.EquipType.Weapon)
        {
            animator.runtimeAnimatorController = defaultAnimator;
        }
    }

    public void useItem(ItemData itemData, int index = -1)
    {
        if (inventory.list.Contains(itemData))
        {
            Item instanceItem = Instantiate(itemData.prefab);
            IUsable usableItem = instanceItem as IUsable;
            usableItem.Use();
            OnChangeUseQuestUpdate?.Invoke(itemData);
            RemoveItemFromInventory(itemData, index);
            Destroy(instanceItem.gameObject);
        }
    }

    public void Hit(int damage)
    {
        if (currentHp <= 0)
            return;

        CurrentHP -= damage;
        GameManager.Ui.SetFloating(gameObject, -damage, new Color(1,0,0,1));
    }

    NPC prevNpc;
    Vector3 lookrot;

    public void ScanNpc()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("NPC"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dirTarget) >= Mathf.Cos(90 * 0.5f * Mathf.Deg2Rad))
                {
                    NPC npc = collider.GetComponent<NPC>();

                    if (UnityEngine.Input.GetKeyDown(KeyCode.F))
                    {
                        playerInput.enabled = false;
                        if (mouseController.mouseSensitivity != 0)
                            mouseController.prevMousSens = mouseController.mouseSensitivity;
                        mouseController.mouseSensitivity = 0f;

                        lookrot = npc.transform.rotation.eulerAngles;
                        animator.SetBool("Talk", true);
                        npc.transform.LookAt(transform.position);

                        npc.OpenTalk();

                        prevNpc = npc;
                    }
                }
                else
                {
                    ExitNpc();
                }
            }
        }
        else
        {
            ExitNpc();
        }
    }

    void ExitNpc()
    {
        if (prevNpc != null)
        {
            if (GameObject.Find("TalkUI Panel").IsValid())
                GameObject.Find("TalkUI Panel").SetActive(false);
            prevNpc.transform.rotation = Quaternion.Euler(lookrot);
            prevNpc.animator.SetBool("Talk", false);
        }

        animator.SetBool("Talk", false);
        prevNpc = null;
    }

    public void ScanMonster()
    {
        if (monsterUI == null)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Monster"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.tag == "Boss")
                    return;

                Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

                Monster monster = collider.GetComponent<Monster>();

                if (Vector3.Dot(transform.forward, dirTarget) >= Mathf.Cos(150 * 0.5f * Mathf.Deg2Rad) && monster.currHp > 0)
                {
                    monsterUI.animator.SetBool("Active", true);
                    monsterUI.animator.SetBool("UnActive", false);

                    monsterUI.hpBar.maxValue = monster.data.maxHp;
                    monsterUI.hpBar.value = monster.currHp;
                    monsterUI.hpBar.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{monster.currHp}/{monster.data.maxHp}";
                    monsterUI.monsterName.text = monster.name;
                }
                else
                {
                    monsterUI.animator.SetBool("Active", false);
                    monsterUI.animator.SetBool("UnActive", true);
                }
            }
        }
        else
        {
            monsterUI.animator.SetBool("Active", false);
            monsterUI.animator.SetBool("UnActive", false);
        }
    }

    void LevelUp()
    {
        level++;
        currentHp = Status.MaxHp;
        exp -= nextLevelExp;

        switch (level)
        {
            case 1:
                nextLevelExp = 1000;
                break;
            case 2:
                nextLevelExp = 1500;
                break;
            case 3:
                nextLevelExp = 2200;
                break;
            case 4:
                nextLevelExp = 3000;
                break;
            case 5:
                nextLevelExp = 4200;
                break;
            case 6:
                nextLevelExp = 5800;
                break;
            case 7:
                nextLevelExp = 7000;
                break;
            case 8:
                nextLevelExp = 9000;
                break;
            case 9:
                nextLevelExp = 11500;
                break;
            case 10:
                nextLevelExp = 14500;
                break;
            case 11:
                nextLevelExp = 19000;
                break;
            case 12:
                nextLevelExp = 25000;
                break;
            case 13:
                nextLevelExp = 32000;
                break;
            case 14:
                nextLevelExp = 43000;
                break;
            case 15:
                nextLevelExp = 60000;
                break;
        }
    }

    void LevelUpEffect()
    {
        GameManager.Sound.PlaySFX("LevelUp");
        GameObject party = GameManager.Resource.Instantiate<GameObject>("Prefabs/Player/PartyParticle", transform.position + (transform.up * 2f), Quaternion.identity);
        Destroy(party, 4f);
    }
}