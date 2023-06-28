using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]
public class PlayerStatusData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Ä³¸¯ÅÍ ½ºÅÝ")]
    [SerializeField] int _maxHP;
    [SerializeField] float _deffense;
    [SerializeField] int _attackPower;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _walkSpeed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _jumpPower;

    public int maxHP { get { return _maxHP; } set { _maxHP = value; } }
    public float deffense { get { return _deffense; } set { _deffense = value; } }
    public int attackPower { get { return _attackPower; } set { _attackPower = value; } }
    public float attackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
    public float walkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public float runSpeed { get { return _runSpeed; } set { _runSpeed = value; } } 
    public float jumpPower { get { return _jumpPower; } set { _jumpPower = value; } }

    [NonSerialized] public int MaxHp;
    [NonSerialized] public float Deffense;
    [NonSerialized] public int AttackPower;
    [NonSerialized] public float AttackSpeed;
    [NonSerialized] public float WalkSpeed;
    [NonSerialized] public float RunSpeed;
    [NonSerialized] public float JumpPower;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        MaxHp = _maxHP;
        Deffense = _deffense;
        AttackPower = _attackPower;
        AttackSpeed = _attackSpeed;
        WalkSpeed = _walkSpeed;
        RunSpeed = _runSpeed;
        JumpPower = _jumpPower;
    }
}