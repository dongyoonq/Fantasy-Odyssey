using System;
using UnityEngine;

[Serializable]
public class Quest
{
    public bool isActive;

    public string name;
    public string title;
    public string description;
    public int expReward;

    public QuestGoal goal;

}