using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPC Data", menuName = "Scriptable Object/Npc Data", order = 100000)]
public class NpcData : ScriptableObject, ISerializationCallbackReceiver
{
    [NonSerialized] public TalkData talkData;
    [NonSerialized] public QuestData quest;

    [NonSerialized] public QuestGiver questNpc;
    [NonSerialized] public bool isCompleteQuest;
    [NonSerialized] public bool isQuestNPC;

    [SerializeField] bool _isQuestNPC;
    [SerializeField] TalkData _talkData;
    [SerializeField] QuestData _quest;


    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        questNpc = null;
        isCompleteQuest = false;
        isQuestNPC = _isQuestNPC;
        talkData = _talkData;
        quest = _quest;
    }
}