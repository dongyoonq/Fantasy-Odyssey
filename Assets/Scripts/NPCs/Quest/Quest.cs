using System;
using UnityEngine;

[Serializable]
public class Quest
{
    [NonSerialized] public bool isActive;

    public string name;
    public string title;
    public string target;
    public string description;
    public int expReward;

    public QuestGoal goal;

}