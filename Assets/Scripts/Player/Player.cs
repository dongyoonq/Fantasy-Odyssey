using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance { get { return instance; } }
    private static Player instance;

    public PlayerController playerController { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public CharacterController controller { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public Inventory inventory { get; private set; }

    public Dictionary<Equipment.EquipmentType, Equipment> wearingEquip { get; private set; }
    public Queue<Input> inputBuffer { get; private set; }

    [SerializeField] PlayerStatusData status;
    public PlayerStatusData Status { get { return status; } }

    [SerializeField] public string playerName;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float JumpPower;

    [NonSerialized] public float MoveSpeed;
    [NonSerialized] public float YSpeed;
    [NonSerialized] public float CurrentHP;

    [SerializeField] public Transform hand;

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

        inventory = new Inventory();
        wearingEquip = new Dictionary<Equipment.EquipmentType, Equipment>();
        inputBuffer = new Queue<Input>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        SetStatus();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
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

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitStateMachine()
    {
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller)); // 등록
        stateMachine.AddState(StateName.ATTACK, new AttackState(controller));
        stateMachine.AddState(StateName.Dash, new DashState(controller));
        stateMachine.AddState(StateName.DashAttack, new DashAttackState(controller));
    }

    void SetStatus()
    {
        CurrentHP = Status.MaxHp;
        Status.AttackPower = Status.attackPower;
        Status.AttackSpeed = Status.attackSpeed;
        Status.Deffense = Status.deffense;
        Status.MaxHp = Status.maxHP;
    }

    // 인벤토리 추가 메서드
    public void AddItemToInventory(Item item)
    {
        this.inventory.list.Add(item);
    }

    // 인벤토리 아이템 제거 메서드
    public void RemoveItemFromInventory(Item item)
    {
        this.inventory.list.Remove(item);
    }

    /// <summary>
    /// 장비 착용 이벤트 핸들러
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="equipment"></param>
    public bool OnEquip(Equipment equipment)
    {
        // 들어온 아이템이 그 부위에 착용중이면 장비를 벗는다.
        if (wearingEquip.ContainsKey(equipment.type))
        {
            UnEquip(wearingEquip[equipment.type]);
        }

        // 인벤토리에 장비를 지우고
        inventory.list.Remove(equipment);
        // 착용한 분위에 이 장비를 착용 시킨다.
        wearingEquip.Add(equipment.type, equipment);
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
    public bool UnEquip(Equipment equipment)
    {
        // 들어온 아이템이없으면 빠져나온다.
        if (equipment == null)
            return false;

        // 착용중인 부위에 아이템이 있으면
        if (wearingEquip.ContainsKey(equipment.type))
        {
            // 인벤토리에 착용중인 장비를 넣어주고
            inventory.list.Add(wearingEquip[equipment.type]);
            // 착용중인 장비를 지워준다.
            wearingEquip.Remove(equipment.type);
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

    // 무기 삭제
    public void UnRegisterWeapon(GameObject weapon)
    {
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