using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("∏ÛΩ∫≈Õ Ω∫≈›")]
    [SerializeField] float _maxHP;
    [SerializeField] float _deffense;
    [SerializeField] int _attackPower;
    [SerializeField] float _attackSpeed;

    public float maxHP { get { return _maxHP; } }
    public float deffense { get { return _deffense; } }
    public int attackPower { get { return _attackPower; } }
    public float attackSpeed { get { return _attackSpeed; } }

    [NonSerialized] public float MaxHp;
    [NonSerialized] public float Deffense;
    [NonSerialized] public int AttackPower;
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