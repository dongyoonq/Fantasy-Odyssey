using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IHitable
{
    public UnityEvent<ItemData> OnChangeShortSlot;
    public UnityEvent OnChangeKillQuestUpdate;
    public UnityEvent<ItemData> OnChangeGatheringQuestUpdate;
    public UnityEvent OnChangedHp;
    public UnityEvent<ItemData, int, int> OnAddItemInventory;
    public UnityEvent<ItemData, int, int> OnRemoveItemInventory;
    public UnityEvent OnLevelUp;
    public UnityEvent OnChangeEquipment;

    public static Player Instance { get { return instance; } }
    private static Player instance;

    public PlayerController playerController { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public CharacterController controller { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public Inventory inventory { get; private set; }
    public InventoryUI inventoryUI { get; set; }
    public QuestUI questUI { get; set; }
    public EquipmentUI equipUI { get; set; }
    public MonsterInfoUI monsterUI { get; set; }
    public ShortSlotUI shortUI { get; set; }
    public List<QuestData> questList { get; private set; }

    public Dictionary<EquipmentData.EquipType, Equipment> wearingEquip { get; private set; }
    public Queue<Input> inputBuffer { get; private set; }

    [SerializeField] PlayerStatusData status;
    public PlayerStatusData Status { get { return status; } }

    [SerializeField] public string playerName;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float JumpPower;
    [SerializeField] float currentHp;
    [SerializeField] int level;
    int exp;
    int nextLevelExp;
    RuntimeAnimatorController defaultAnimator;

    public float CurrentHP { 
        get { return currentHp; } 
        set 
        {
            if (value - currentHp < 0) 
                OnChangedHp?.Invoke();

            currentHp = value;
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
        MoveSpeed = walkSpeed;

        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        wearingEquip = new Dictionary<EquipmentData.EquipType, Equipment>();
        questList = new List<QuestData>();
        inputBuffer = new Queue<Input>();
        controller = GetComponent<CharacterController>();
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
    }

    void Update()
    {
        stateMachine?.UpdateState();

        if (exp >= nextLevelExp)
        {
            LevelUp();
            OnLevelUp?.Invoke();
        }

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
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller)); // ���
        stateMachine.AddState(StateName.Dash, new DashState(controller));
        stateMachine.AddState(StateName.ATTACK, new BaseAttackState(controller));
        stateMachine.AddState(StateName.LAttack, new LeftAttackState(controller));
        stateMachine.AddState(StateName.RAttack, new RightAttackState(controller));
        stateMachine.AddState(StateName.ChargeAttack, new ChargeAttackState(controller));
        stateMachine.AddState(StateName.SkillAttack, new SkillAttackState(controller));
        stateMachine.AddState(StateName.UltAttack, new UltAttackState(controller));
        stateMachine.AddState(StateName.DashAttack, new DashAttackState(controller));
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
    }

    // �κ��丮 �߰� �޼���
    public void AddItemToInventory(ItemData item)
    {
        if (inventory.list.Count(x => x == null) == 0)
        {
            Debug.Log("������ ����á��");
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
    }

    public void AddItemToInventory(ItemData item, int index = 0)
    {
        OnAddItemInventory?.Invoke(item, index, 1);
    }

    // �κ��丮 ������ ���� �޼���
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
    }

    /// <summary>
    /// ��� ���� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param QuestName="sender"></param>
    /// <param QuestName="equipment"></param>
    public bool OnEquip(Equipment equipment, int index = -1)
    {
        // ���� �������� �� ������ �������̸� ��� ���´�.
        if (wearingEquip.ContainsKey(equipment.equipmentData.equipType))
        {
            UnEquip(equipment, index);
            // �� ��� ���� ���� ����
            equipment.ApplyStatusModifier();
            RegisterWeapon(equipment.gameObject);
            OnChangeEquipment?.Invoke();
            return true;    
        }

        // �κ��丮�� ��� �����
        RemoveItemFromInventory(equipment.Data, index);

        // ������ ������ �� ��� ���� ��Ų��.
        wearingEquip.Add(equipment.equipmentData.equipType, equipment);
        // �� ��� ���� ���� ����
        equipment.ApplyStatusModifier();

        RegisterWeapon(equipment.gameObject);
        OnChangeEquipment?.Invoke();
        return true;
    }

    /// <summary>
    /// ��� �������� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param QuestName="sender"></param>
    /// <param QuestName="equipment"></param>
    public bool UnEquip(Equipment equipment, int index = -1)
    {
        // ���� �������̾����� �������´�.
        if (equipment == null)
            return false;

        // �������� ������ �������� ������
        if (wearingEquip.ContainsKey(equipment.equipmentData.equipType))
        {
            // �������� ��� �����ش�.
            Destroy(wearingEquip[equipment.equipmentData.equipType].gameObject);
            wearingEquip.Remove(equipment.equipmentData.equipType);

            // ������ ������ �� ��� ���� ��Ų��.
            wearingEquip.Add(equipment.equipmentData.equipType, equipment);

            // �κ��丮�� ��� �����
            RemoveItemFromInventory(inventory.list[index], index);

            // �κ��丮�� ���� ���� ��� �־��ְ�
            inventory.list[index] = equipment.Data;
            AddItemToInventory(equipment.Data, index);

            // ���� ������
            equipment.RemoveStatusModifier();

            UnRegisterWeapon(equipment.gameObject);

            OnChangeEquipment?.Invoke();
            return true;
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
            Debug.Log("������ ����á��");
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

        // �������� ��� �����ش�.
        Destroy(wearingEquip[equipment.equipmentData.equipType].gameObject);
        wearingEquip.Remove(equipment.equipmentData.equipType);

        // �κ��丮�� ���� ���� ��� �־��ְ�
        inventory.list[index] = equipment.Data;
        AddItemToInventory(equipment.Data, index);

        // ���� ������
        equipment.RemoveStatusModifier();
        UnRegisterWeapon(equipment.gameObject);

        OnChangeEquipment?.Invoke();
        return true;
    }

    // ���� ���
    public void RegisterWeapon(GameObject weapon)
    {
        if (wearingEquip.ContainsKey(EquipmentData.EquipType.Weapon))
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

    // ���� ����
    public void UnRegisterWeapon(GameObject weapon)
    {
        animator.runtimeAnimatorController = defaultAnimator;    
        weapon.gameObject.SetActive(false);
    }

    public void useItem(ItemData itemData, int index = -1)
    {
        if (inventory.list.Contains(itemData))
        {
            Item instanceItem = Instantiate(itemData.prefab);
            IUsable usableItem = instanceItem as IUsable;
            usableItem.Use();
            RemoveItemFromInventory(itemData, index);
            Destroy(instanceItem.gameObject);
        }
    }

    public void Hit(int damamge)
    {
        CurrentHP -= damamge;
    }

    NPC prevNpc;

    public void ScanNpc()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, LayerMask.GetMask("NPC")))
        {
            NPC npc = hit.collider.GetComponent<NPC>();

            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                npc.OpenTalk();

                prevNpc = npc;
            }
        }
        else
        {
            if (prevNpc != null)
            {
                if (GameObject.Find("TalkUI Panel").IsValid())
                    GameObject.Find("TalkUI Panel").SetActive(false);
            }

            prevNpc = null;
        }
    }

    public void ScanMonster()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Monster"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

                Monster monster = collider.GetComponent<Monster>();

                if (Vector3.Dot(transform.forward, dirTarget) >= Mathf.Cos(150 * 0.5f * Mathf.Deg2Rad) && monster.currHp > 0)
                {
                    monsterUI.animator.SetBool("Active", true);
                    monsterUI.animator.SetBool("UnActive", false);

                    monsterUI.hpBar.maxValue = monster.data.MaxHp;
                    monsterUI.hpBar.value = monster.currHp;
                    monsterUI.hpBar.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{monster.currHp}/{monster.data.MaxHp}";
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
}