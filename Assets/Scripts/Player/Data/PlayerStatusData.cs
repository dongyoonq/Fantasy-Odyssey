using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]
public class PlayerStatusData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Ä³¸¯ÅÍ ½ºÅÝ")]
    [SerializeField] float _maxHP;
    [SerializeField] float _deffense;
    [SerializeField] float _attackPower;
    [SerializeField] float _attackSpeed;

    public float maxHP { get { return _maxHP; } }
    public float deffense { get { return _deffense; } }
    public float attackPower { get { return _attackPower; } }
    public float attackSpeed { get { return _attackSpeed; } }

    [NonSerialized] public float MaxHp;
    [NonSerialized] public float Deffense;
    [NonSerialized] public float AttackPower;
    [NonSerialized] public float AttackSpeed;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        MaxHp = _maxHP;
        Deffense = _deffense;
        AttackPower = _attackPower;
        AttackSpeed = _attackSpeed;
    }
}