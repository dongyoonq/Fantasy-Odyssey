using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest Data", menuName = "Scriptable Object/Quest Data", order = 100000)]
public class QuestData : ScriptableObject, ISerializationCallbackReceiver
{
    [NonSerialized] public bool isActive;

    [NonSerialized] public string QuestName;     // QuestData Name or QuestData Id(Identifier)
    [NonSerialized] public string Title;
    [NonSerialized] public string Target;
    [NonSerialized, TextArea] public string Description;
    [NonSerialized] public int ExpReward;
    [NonSerialized] public QuestGoal Goal;

    bool _isActive = false;
    [SerializeField] string _questName;     // QuestData Name or QuestData Id(Identifier)
    [SerializeField] string _title;
    [SerializeField] string _target;
    [SerializeField, TextArea] string _description;
    [SerializeField] int _expReward;
    [SerializeField] QuestGoal _goal;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        isActive = _isActive;
        QuestName = _questName;
        Title = _title;
        Target = _target;
        Description = _description;
        ExpReward = _expReward;
        Goal = _goal;
    }
}