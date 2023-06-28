using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPC Data", menuName = "Scriptable Object/Npc Data", order = 100000)]
public class NpcData : ScriptableObject
{
    //[NonSerialized] public TalkData talkData;
    //[NonSerialized] public QuestData quest;

    [NonSerialized] public QuestGiver questNpc;
    [NonSerialized] public bool isCompleteQuest;
    public bool isQuestNPC;

    [SerializeField] public TalkData talkData;
    [SerializeField] public  QuestData quest;
}