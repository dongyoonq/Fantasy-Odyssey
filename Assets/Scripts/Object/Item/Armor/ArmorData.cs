using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Data", menuName = "Scriptable Object/Armor Data", order = 100000000)]
public class ArmorData : EquipmentData
{
    public int MaxHP { get { return maxHp; } set { maxHp = value; } }
    public int Deffense { get { return deffense; } set { deffense = value; } }
    public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }

    [Header("방어구 정보")]
    [SerializeField] int maxHp;
    [SerializeField] int deffense;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpPower;
}