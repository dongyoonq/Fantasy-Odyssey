using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IHitable
{
    public UnityEvent OnChangedHp;
    public UnityEvent<ItemData, int> OnAddItemInventory;
    public UnityEvent<ItemData, int> OnRemoveItemInventory;

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
    public float CurrentHP { get { return currentHp; } set { currentHp = value; OnChangedHp?.Invoke(); } }

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

        OnAddItemInventory?.Invoke(item, index);
    }

    public void AddItemToInventory(ItemData item, int index = 0)
    {
        OnAddItemInventory?.Invoke(item, index);
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

        OnRemoveItemInventory?.Invoke(item, index);
        this.inventory.list[index] = null;
        inventory.onChangeInvntory?.Invoke();
    }

    /// <summary>
    /// ��� ���� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
    public bool OnEquip(Equipment equipment, int index = -1)
    {
        // ���� �������� �� ������ �������̸� ��� ���´�.
        if (wearingEquip.ContainsKey(equipment.equipmentData.equipType))
        {
            UnEquip(equipment, index);
            // �� ��� ���� ���� ����
            equipment.ApplyStatusModifier();
            RegisterWeapon(equipment.gameObject);
            return true;    
        }

        // �κ��丮�� ��� �����
        RemoveItemFromInventory(equipment.Data, index);

        // ������ ������ �� ��� ���� ��Ų��.
        wearingEquip.Add(equipment.equipmentData.equipType, equipment);
        // �� ��� ���� ���� ����
        equipment.ApplyStatusModifier();
        RegisterWeapon(equipment.gameObject);
        return true;
    }

    /// <summary>
    /// ��� �������� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
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
            //Destroy(equipment);
            return true;
        }
        else
        {
            return false;
        }
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
        weapon.gameObject.SetActive(false);
    }

    public void useItem(ItemData item)
    {
        if (inventory.list.Contains(item))
        {
            //item.Use();
            RemoveItemFromInventory(item);
        }
    }

    public void Hit(int damamge)
    {
        CurrentHP -= damamge;
    }
}