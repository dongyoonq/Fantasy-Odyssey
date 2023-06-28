using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeMonster Data", menuName = "Scriptable Object/RangeMonster Data", order = 1)]
public class RangeMonsterData : ScriptableObject
{
    [Header("���Ÿ����� ����")]
    [NonSerialized] public string id;
    [SerializeField] public float detectRange;
    [SerializeField] public float attackDistance;
    [SerializeField] public float angle;
    [SerializeField] public int attackDamage;
}