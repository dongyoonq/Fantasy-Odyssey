using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Player player { get; private set; }
    public Vector3 moveDir { get; set; }
    public Vector3 MouseDirection { get; private set; }

    [Header("땅 체크")]
    [SerializeField, Tooltip("캐릭터가 땅에 붙어 있는지 확인하기 위한 CheckBox 시작 지점입니다.")]
    Transform groundCheck;
    private int groundLayer;
    public bool isGrounded;

    [SerializeField] public float runStepRange;
    [SerializeField] public float walkStepRange;

    public BaseAttackState attackState;

    private void OnEnable()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        //Cursor.lockState = CursorLockMode.None;
    }

    void Start()
    {
        attackState = Player.Instance.stateMachine.GetState(StateName.ATTACK) as BaseAttackState;
        attackState.ComboCount = 0;
        attackState.IsLeftAttack = false;
        player = GetComponent<Player>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }

    void Update()
    {
        Fall();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !Player.Instance.animator.GetBool("Dash"))
        {
            Jump();
        }
    }

    void Jump()
    {
        player.animator.SetTrigger("Jump");
        player.YSpeed = player.Status.JumpPower;
    }

    void Fall()
    {
        if (isGrounded && player.YSpeed < 0)
            player.YSpeed = 0;
        else
            player.YSpeed += Physics.gravity.y * Time.deltaTime;

        if (Player.Instance.controller.enabled)
            player.controller.Move(Vector3.up * player.YSpeed * Time.deltaTime);
    }

    [NonSerialized] public bool OnRunKey;

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.MoveSpeed = player.Status.RunSpeed;
            OnRunKey = true;
        }
        else
        {
            player.MoveSpeed = player.Status.WalkSpeed;
            OnRunKey = false;
        }
    }

    public bool isCharging = false;

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)         // 차지 공격
            {
                isCharging = true;
            }
            else if (context.interaction is PressInteraction)   // 일반 공격
            {
                if (GetWeapon() == null)
                {
                    bool isNonWeaponAttack = !attackState.IsLeftAttack &&
                          (attackState.ComboCount < 3);

                    if (isNonWeaponAttack)
                    {
                        player.stateMachine.ChangeState(StateName.ATTACK);
                    }

                    return;
                }

                bool isAvailableAttack = !attackState.IsLeftAttack &&
                          (GetWeapon()?.ComboCount < GetWeapon()?.weaponData.MaxCombo);

                if (isAvailableAttack)
                {
                    player.inputBuffer.Enqueue(Player.Input.LAttack);
                    player.stateMachine.ChangeState(StateName.ATTACK);
                    return;
                }

            }
        }
        else if (context.canceled)
        {
            if (isCharging && GetWeapon() != null)
            {
                player.stateMachine.ChangeState(StateName.ATTACK);
                isCharging = false;
            }
        }
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !attackState.IsLeftAttack && GetWeapon() != null)
        {
            if (context.interaction is PressInteraction)
            {
                player.inputBuffer.Enqueue(Player.Input.RAttack);
                player.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }

    public bool isUltAttack = false;
    Coroutine skillCoolTime;

    public void OnUltAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !attackState.IsLeftAttack && GetWeapon() != null && !Player.Instance.animator.GetBool("Dash"))
        {
            if (context.interaction is PressInteraction)
            {
                if (skillCoolTime == null)
                {
                    Debug.Log("쿨타임 시작");
                    skillCoolTime = StartCoroutine(UltCoolTimer());
                    isUltAttack = true;
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }

                Debug.Log("쿨타임 중");
            }
        }
    }

    private IEnumerator UltCoolTimer()
    {
        float currentTime = 0f;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= GetWeapon()?.weaponData.CoolTimeSkill)
                break;

            yield return null;
        }

        if (skillCoolTime != null)
        {
            StopCoroutine(skillCoolTime);
            skillCoolTime = null;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && context.interaction is PressInteraction && isGrounded && !Player.Instance.animator.GetBool("Dash")
            && !Player.Instance.animator.GetBool("IsDashAttack") && !Player.Instance.animator.GetBool("IsLeftAttack") &&
            !Player.Instance.animator.GetBool("IsRightAttack") && !Player.Instance.animator.GetBool("IsChargingAttack") && !Player.Instance.animator.GetBool("IsSkillAttack") && !Player.Instance.animator.GetBool("IsUltAttack"))
        {
            player.inputBuffer.Enqueue(Player.Input.Dash);

            player.stateMachine.ChangeState(StateName.Dash);
        }
    }

    public Weapon GetWeapon()
    {
        if (player.wearingEquip.ContainsKey(EquipmentData.EquipType.Weapon))
        {
            Equipment equipment = player.wearingEquip[EquipmentData.EquipType.Weapon];
            Weapon weapon = equipment as Weapon;
            return weapon;
        }

        return null;
    }

    protected bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity,
               groundLayer);
    }

    public void OnShortKey_1(InputAction.CallbackContext context)
    {
        if (context.performed && player.shortUI.slots[0].usableItem != null && player.shortUI.slots[0].amount > 0)
        {
            player.useItem(player.shortUI.slots[0].usableItem);
        }
    }

    public void OnShortKey_2(InputAction.CallbackContext context)
    {
        if (context.performed && player.shortUI.slots[1].usableItem != null && player.shortUI.slots[1].amount > 0)
        {
            player.useItem(player.shortUI.slots[1].usableItem);
        }
    }

    public void OnShortKey_3(InputAction.CallbackContext context)
    {
        if (context.performed && player.shortUI.slots[2].usableItem != null && player.shortUI.slots[2].amount > 0)
        {
            player.useItem(player.shortUI.slots[2].usableItem);
        }
    }

    public void OnShortKey_4(InputAction.CallbackContext context)
    {
        if (context.performed && player.shortUI.slots[3].usableItem != null && player.shortUI.slots[3].amount > 0)
        {
            player.useItem(player.shortUI.slots[3].usableItem);
        }
    }

    public void OnShortKey_5(InputAction.CallbackContext context)
    {
        if (context.performed && player.shortUI.slots[4].usableItem != null && player.shortUI.slots[4].amount > 0)
        {
            player.useItem(player.shortUI.slots[4].usableItem);
        }
    }

    [SerializeField] bool debug;

    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkStepRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, runStepRange);
    }
}