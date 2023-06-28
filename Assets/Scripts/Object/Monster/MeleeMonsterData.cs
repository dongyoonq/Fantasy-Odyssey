using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeMonster Data", menuName = "Scriptable Object/MeleeMonster Data", order = 1)]
public class MeleeMonsterData : ScriptableObject
{
    [Header("�������� ����")]
    [NonSerialized] public string id;
    [SerializeField] public float detectRange;
    [SerializeField] public float attackDistance;
    [SerializeField] public float angle;
    [SerializeField] public int attackDamage;
}