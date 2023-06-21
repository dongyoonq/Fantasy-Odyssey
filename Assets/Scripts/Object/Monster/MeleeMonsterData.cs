using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeMonster Data", menuName = "Scriptable Object/MeleeMonster Data", order = 1)]
public class MeleeMonsterData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("근접공격 정보")]
    [SerializeField] float _detectRange;
    [SerializeField] float _attackDistance;
    [SerializeField] int _attackDamage;

    [NonSerialized] public float DetectRange;
    [NonSerialized] public float AttackDistance;
    [NonSerialized] public int AttackDamage;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        DetectRange = _detectRange;
        AttackDistance = _attackDistance;
        AttackDamage = _attackDamage;
    }
}