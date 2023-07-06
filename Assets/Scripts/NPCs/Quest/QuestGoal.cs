using System;

[Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public ItemData item;
    public bool isCompleteTalked;
    public NpcData targetNpc;

    public int requiredAmount;
    public int currentAmount;

    public bool IsReached()
    {
        if (goalType == GoalType.Use)
            return currentAmount <= requiredAmount;

        if (goalType == GoalType.Talk)
            return isCompleteTalked;

        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled()
    {
        if (goalType == GoalType.Kill)
            currentAmount++;
    }

    public void ItemCollected()
    {
        if (goalType == GoalType.Gathering)
            currentAmount++;
    }

    public void ItemUse()
    {
        if (goalType == GoalType.Use)
            currentAmount--;
    }

    public void TalkNpc()
    {
        if (goalType == GoalType.Talk)
            isCompleteTalked = true;
    }
}

public enum GoalType
{
    Kill,
    Gathering,
    Use,
    Talk,
}