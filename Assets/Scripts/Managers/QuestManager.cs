using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    GameObject contentArea;
    GameObject questListUI;

    private void Start()
    {
        questListUI = GameObject.Find("QuestUI").transform.GetChild(0).gameObject;
        contentArea = questListUI.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        Player.Instance.OnChangeKillQuestUpdate.AddListener(UpdateKillQuest);
        Player.Instance.OnChangeGatheringQuestUpdate.AddListener(UpdateGatheringQuest);
    }

    void UpdateKillQuest()
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Kill QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].QuestName == "SpiderKill" &&
                Player.Instance.questList[i].Goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].Goal.EnemyKilled();

                if (Player.Instance.questList[i].Goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }

            }
        }
    }

    void UpdateGatheringQuest(ItemData item)
    {
        Debug.Log(item.itemName);
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Gathering QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].QuestName == "SpiderGathering" &&
                Player.Instance.questList[i].Goal.goalType == GoalType.Gathering && item.itemName == "SpiderBooty")
            {
                Player.Instance.questList[i].Goal.ItemCollected();

                if (Player.Instance.questList[i].Goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }
}
