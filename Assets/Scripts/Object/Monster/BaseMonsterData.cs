using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseMonster Data", menuName = "Scriptable Object/BaseMonster Data", order = 1)]
public class BaseMonsterData : ScriptableObject
{
    [Header("몬스터 정보")]
    [SerializeField] public string id;
    [SerializeField] public List<AgressiveMonsterData> agressiveMonsterData;
    [SerializeField] public List<MeleeMonsterData> meleeMonsterData;
    [SerializeField] public List<RangeMonsterData> rangeMonsterData;
    [SerializeField] public List<ItemData> dropTable;
    [SerializeField] public int dropExp;
    [SerializeField] public int maxHp;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotSpeed;
}