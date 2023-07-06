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

    public bool isQuestNPC;
    public bool isTargetNpc;

    [SerializeField] public TalkData talkData;
    [SerializeField] public QuestData quest;
}