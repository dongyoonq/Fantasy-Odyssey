using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateMachine;

public class Player : MonoBehaviour
{
    public static Player Instance { get { return instance; } }
    public StateMachine stateMachine { get; private set; }
    public CharacterController controller { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public WeaponManager weaponManager { get; private set; }

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

    [Header("캐릭터 스탯")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float deffense;
    [SerializeField] protected float attackPower;
    [SerializeField] protected float walkspeed;
    [SerializeField] protected float runspeed;
    [SerializeField] protected float jumpPower;
    [SerializeField] protected float ySpeed;

    void Awake()
    {
        MoveSpeed = walkspeed;

        if (instance == null)
        {
            instance = this;
            weaponManager = new WeaponManager(hand);
            weaponManager.unRegisterWeapon = (weapon) => {  Destroy(weapon); };
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
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller)); // 등록
        stateMachine.AddState(StateName.ATTACK, new AttackState(controller));

    }
}