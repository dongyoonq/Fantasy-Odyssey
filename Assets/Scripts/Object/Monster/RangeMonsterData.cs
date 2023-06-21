using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeMonster Data", menuName = "Scriptable Object/RangeMonster Data", order = 1)]
public class RangeMonsterData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("���Ÿ����� ����")]
    [SerializeField] float _detectRange;
    [SerializeField] float _attackDistance;
    [SerializeField, Range(0f, 360f)] float _angle;
    [SerializeField] int _attackDamage;

    [NonSerialized] public float DetectRange;
    [NonSerialized] public float AttackDistance;
    [NonSerialized] public float Angle;
    [NonSerialized] public int AttackDamage;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        DetectRange = _detectRange;
        AttackDistance = _attackDistance;
        AttackDamage = _attackDamage;
        Angle = _angle;
    }
}