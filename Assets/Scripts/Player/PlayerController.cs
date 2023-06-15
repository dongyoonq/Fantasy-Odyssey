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
    public Vector3 moveDir { get; private set; }
    public Vector3 MouseDirection { get; private set; }

    [Header("땅 체크")]
    [SerializeField, Tooltip("캐릭터가 땅에 붙어 있는지 확인하기 위한 CheckBox 시작 지점입니다.")]
    Transform groundCheck;
    private int groundLayer;
    private bool isGrounded;

    private void OnEnable()
    {
       // Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
      //  Cursor.lockState = CursorLockMode.None;
    }

    void Start()
    {
        AttackState.ComboCount = 0;
        AttackState.IsAttack = false;
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
        if (context.started && isGrounded)
            Jump();
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)         // 차지 공격
            {

            }

            else if (context.interaction is PressInteraction)   // 일반 공격
            {
                if (GetWeapon() == null)
                {
                    bool isNonWeaponAttack = !AttackState.IsAttack &&
                          (AttackState.ComboCount < 3);

                    if (isNonWeaponAttack)
                        player.stateMachine.ChangeState(StateName.ATTACK);
                }

                bool isAvailableAttack = !AttackState.IsAttack &&
                          (GetWeapon()?.ComboCount < 3);


                if (isAvailableAttack)
                {
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }

            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // .. change state
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