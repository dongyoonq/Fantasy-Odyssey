using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AgressiveMonster Data", menuName = "Scriptable Object/AgressiveMonster Data", order = 1)]
public class AgressiveMonsterData : ScriptableObject
{
    [Header("공격적인 몬스터 정보")]
    [NonSerialized] public string id;
    [SerializeField] public float detectRange;
    [SerializeField] public float detectAngle;
}