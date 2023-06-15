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
    private bool isGrounded;

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
        AttackState.ComboCount = 0;
        AttackState.IsLeftAttack = false;
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
            player.inputBuffer.Enqueue(Player.Input.Jump);
            Jump();
        }
    }

    void Jump()
    {
        player.animator.SetTrigger("Jump");
        player.YSpeed = player.JumpPower;
    }

    void Fall()
    {
        player.YSpeed += Physics.gravity.y * Time.deltaTime;

        if (isGrounded && player.YSpeed < 0)
            player.YSpeed = 0;

        player.controller.Move(Vector3.up * player.YSpeed * Time.deltaTime);
    }

    [NonSerialized] public bool OnRunKey;

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.MoveSpeed = player.runSpeed;
            OnRunKey = true;
        }
        else
        {
            player.MoveSpeed = player.walkSpeed;
            OnRunKey = false;
        }
    }

    public static bool isCharging = false;

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
                    bool isNonWeaponAttack = !AttackState.IsLeftAttack &&
                          (AttackState.ComboCount < 3);

                    if (isNonWeaponAttack)
                    {
                        player.stateMachine.ChangeState(StateName.ATTACK);
                    }

                    return;
                }

                bool isAvailableAttack = !AttackState.IsLeftAttack &&
                          (GetWeapon()?.ComboCount < GetWeapon()?.WeaponData.MaxCombo);

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
        if (context.performed && !AttackState.IsLeftAttack && GetWeapon() != null)
        {
            if (context.interaction is PressInteraction)
            {
                player.inputBuffer.Enqueue(Player.Input.RAttack);
                player.stateMachine.ChangeState(StateName.ATTACK);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && context.interaction is PressInteraction && !Player.Instance.animator.GetBool("Dash") && !Player.Instance.animator.GetBool("IsDashAttack") && !Player.Instance.animator.GetBool("IsLeftAttack"))
        {
            player.inputBuffer.Enqueue(Player.Input.Dash);
            // 대시 입력을 막아야 하는 상황이 있을 경우 return;
            player.stateMachine.ChangeState(StateName.Dash);
        }
    }

    public Weapon GetWeapon()
    {
        if (player.wearingEquip.ContainsKey(Equipment.EquipmentType.Weapon))
        {
            Equipment equipment = player.wearingEquip[Equipment.EquipmentType.Weapon];
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
}