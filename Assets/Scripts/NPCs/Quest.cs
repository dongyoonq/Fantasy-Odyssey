using System;

public class Quest
{
    [NonSerialized] public bool isActive;
    [NonSerialized] public bool isCompleteQuest;

    QuestData questData;
}