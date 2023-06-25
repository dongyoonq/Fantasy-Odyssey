using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IHitable
{
    public UnityEvent OnChangedHp;
    public UnityEvent<ItemData, int, int> OnAddItemInventory;
    public UnityEvent<ItemData, int, int> OnRemoveItemInventory;
    public UnityEvent OnLevelUp;

    public static Player Instance { get { return instance; } }
    private static Player instance;

    public PlayerController playerController { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public CharacterController controller { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public Inventory inventory { get; private set; }
    public InventoryUI inventoryUI { get; set; }

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

    public float CurrentHP { get { return currentHp; } set { currentHp = value; OnChangedHp?.Invoke(); } }
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
        inputBuffer = new Queue<Input>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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

    // 인벤토리 추가 메서드
    public void AddItemToInventory(ItemData item)
    {
        if (inventory.list.Count(x => x == null) == 0)
        {
            Debug.Log("가방이 가득찼다");
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

        OnAddItemInventory?.Invoke(item, index, 1);
    }

    public void AddItemToInventory(ItemData item, int index = 0)
    {
        OnAddItemInventory?.Invoke(item, index, 1);
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

        inventory.onChangeInventory?.Invoke();
    }

    /// <summary>
    /// 장비 착용 이벤트 핸들러
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
    public bool OnEquip(Equipment equipment, int index = -1)
    {
        // 들어온 아이템이 그 부위에 착용중이면 장비를 벗는다.
        if (wearingEquip.ContainsKey(equipment.equipmentData.equipType))
        {
            UnEquip(equipment, index);
            // 그 장비에 대한 스텟 적용
            equipment.ApplyStatusModifier();
            RegisterWeapon(equipment.gameObject);
            return true;    
        }

        // 인벤토리에 장비를 지우고
        RemoveItemFromInventory(equipment.Data, index);

        // 착용한 분위에 이 장비를 착용 시킨다.
        wearingEquip.Add(equipment.equipmentData.equipType, equipment);
        // 그 장비에 대한 스텟 적용
        equipment.ApplyStatusModifier();

        RegisterWeapon(equipment.gameObject);
        return true;
    }

    /// <summary>
    /// 장비 착용해제 이벤트 핸들러
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
    public bool UnEquip(Equipment equipment, int index = -1)
    {
        // 들어온 아이템이없으면 빠져나온다.
        if (equipment == null)
            return false;

        // 착용중인 부위에 아이템이 있으면
        if (wearingEquip.ContainsKey(equipment.equipmentData.equipType))
        {
            // 착용중인 장비를 지워준다.
            Destroy(wearingEquip[equipment.equipmentData.equipType].gameObject);
            wearingEquip.Remove(equipment.equipmentData.equipType);

            // 착용한 분위에 이 장비를 착용 시킨다.
            wearingEquip.Add(equipment.equipmentData.equipType, equipment);

            // 인벤토리에 장비를 지우고
            RemoveItemFromInventory(inventory.list[index], index);

            // 인벤토리에 착용 중인 장비를 넣어주고
            inventory.list[index] = equipment.Data;
            AddItemToInventory(equipment.Data, index);

            // 스텟 미적용
            equipment.RemoveStatusModifier();

            UnRegisterWeapon(equipment.gameObject);
            //Destroy(equipment);
            return true;
        }
        else
        {
            return false;
        }
    }

    // 무기 등록
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

    // 무기 삭제
    public void UnRegisterWeapon(GameObject weapon)
    {
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