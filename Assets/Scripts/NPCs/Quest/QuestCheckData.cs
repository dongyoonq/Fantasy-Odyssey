using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "QuestCheck Data", menuName = "Scriptable Object/Quest Check Data", order = 100000)]
public class QuestCheckData : ScriptableObject
{
    [SerializeField] public QuestData questData;
    [SerializeField] public BaseMonsterData targetMonster;
    [SerializeField] public ItemData targetItem;
}