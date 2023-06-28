using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AgressiveMonster Data", menuName = "Scriptable Object/AgressiveMonster Data", order = 1)]
public class AgressiveMonsterData : ScriptableObject
{
    [Header("�������� ���� ����")]
    [NonSerialized] public string id;
    [SerializeField] public float detectRange;
}