using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance { get { return instance; } }
    public StateMachine stateMachine { get; private set; }
    public CharacterController controller { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public Inventory inventory { get; private set; }
    public Dictionary<Equipment.EquipmentType, Equipment> wearingEquip { get; private set; }

    private static Player instance;

    [NonSerialized] public float MoveSpeed;
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float AttackPower { get { return attackPower; } }
    public float Deffense { get { return deffense; } }
    public float WalkSpeed { get { return walkspeed; } set { walkspeed = value; } }
    public float RunSpeed { get { return runspeed; } set { runspeed = value; } }
    public float JumpPower { get { return jumpPower; } }
    public float YSpeed { get { return ySpeed; } set { ySpeed = value; } }


    [SerializeField] Transform hand;

    [Header("ĳ���� ����")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float deffense;
    [SerializeField] protected float attackPower;
    [SerializeField] protected float walkspeed;
    [SerializeField] protected float runspeed;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float ySpeed;

    [SerializeField] GameObject weapon;

    void Awake()
    {
        MoveSpeed = walkspeed;

        if (instance == null)
        {
            instance = this;
            inventory = new Inventory();
            wearingEquip = new Dictionary<Equipment.EquipmentType, Equipment>();
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    void Start()
    {
        // Test
        Equipment none = Resources.Load<Equipment>("Prefabs/None");
        Equipment instnace = Instantiate(none);
        AddItemToInventory(instnace);
        OnEquip(instnace);

        InitStateMachine();
    }

    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    public void OnUpdateStat(float maxHP, float currentHP, float deffense, float walkspeed, float runspeed, float attackPower, float jumpPower)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.deffense = deffense;
        this.walkspeed = walkspeed;
        this.runspeed = runspeed;
        this.attackPower = attackPower;
        this.jumpPower = jumpPower;
    }

    private void InitStateMachine()
    {
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller)); // ���
        stateMachine.AddState(StateName.ATTACK, new AttackState(controller));

    }

    // �κ��丮 �߰� �޼���
    public void AddItemToInventory(Item item)
    {
        this.inventory.list.Add(item);
    }

    // �κ��丮 ������ ���� �޼���
    public void RemoveItemFromInventory(Item item)
    {
        this.inventory.list.Remove(item);
    }

    /// <summary>
    /// ��� ���� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
    public bool OnEquip(Equipment equipment)
    {
        // ���� �������� �� ������ �������̸� ��� ���´�.
        if (wearingEquip.ContainsKey(equipment.type))
        {
            UnEquip(wearingEquip[equipment.type]);
        }

        // �κ��丮�� ��� �����
        inventory.list.Remove(equipment);
        // ������ ������ �� ��� ���� ��Ų��.
        wearingEquip.Add(equipment.type, equipment);
        // �� ��� ���� ���� ����
        equipment.ApplyStatusModifier(this);
        RegisterWeapon(equipment.gameObject);

        return true;
    }

    /// <summary>
    /// ��� �������� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
    public bool UnEquip(Equipment equipment)
    {
        // ���� �������̾����� �������´�.
        if (equipment == null)
            return false;

        // �������� ������ �������� ������
        if (wearingEquip.ContainsKey(equipment.type))
        {
            // �κ��丮�� �������� ��� �־��ְ�
            inventory.list.Add(wearingEquip[equipment.type]);
            // �������� ��� �����ش�.
            wearingEquip.Remove(equipment.type);
            // ���� ������
            equipment.RemoveStatusModifier(this);

            UnRegisterWeapon(equipment.gameObject);
            //Destroy(equipment);
            return true;
        }
        else
        {
            return false;
        }
    }

    public Action<GameObject> unRegisterWeapon { get; set; }

    // ���� ���
    public void RegisterWeapon(GameObject weapon)
    {
        if (wearingEquip.ContainsKey(Equipment.EquipmentType.Weapon))
        {
            Weapon weaponInfo = weapon.GetComponent<Weapon>();
            weapon.transform.SetParent(hand);
            weapon.transform.localPosition = weaponInfo.WeaponData.localPosition;
            weapon.transform.localEulerAngles = weaponInfo.WeaponData.localRotation;
            weapon.transform.localScale = weaponInfo.WeaponData.localScale;
            animator.runtimeAnimatorController = weaponInfo.WeaponData.WeaponAnimator;
            weapon.gameObject.SetActive(true);
        }
    }

    // ���� ����
    public void UnRegisterWeapon(GameObject weapon)
    {
        //unRegisterWeapon.Invoke(weapon.gameObject);
        weapon.gameObject.SetActive(false);
    }

    public void useItem(Item item)
    {
        if (inventory.list.Contains(item))
        {
            item.Use();
            RemoveItemFromInventory(item);
        }
    }
}