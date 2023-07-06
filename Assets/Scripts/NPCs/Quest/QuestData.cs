using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest Data", menuName = "Scriptable Object/Quest Data", order = 100000)]
public class QuestData : ScriptableObject, ISerializationCallbackReceiver
{
    [NonSerialized] public string id;

    [SerializeField] public string questName;     // QuestData Name or QuestData Id(Identifier)
    [SerializeField] public string title;
    [SerializeField] public string target;
    [SerializeField, TextArea] public string description;
    [SerializeField] public int expReward;
    [SerializeField] public QuestGoal goal;

    public void OnAfterDeserialize()
    {
        //
    }

    public void OnBeforeSerialize()
    {
        //
    }
}