using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    GameObject contentArea;
    public GameObject questListUI;

    private void Start()
    {
        questListUI = GameObject.Find("Quest_list").gameObject;
        contentArea = questListUI.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        Player.Instance.OnChangeKillQuestUpdate.AddListener(UpdateKillQuest);
        Player.Instance.OnChangeGatheringQuestUpdate.AddListener(UpdateGatheringQuest);
    }

    void UpdateKillQuest()
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Kill Quest
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].name == "SpiderKill" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].goal.EnemyKilled();

                if (Player.Instance.questList[i].goal.IsReached())
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
            // Spider Gathering Quest
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].name == "SpiderGathering" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Gathering && item.itemName == "SpiderBooty")
            {
                Player.Instance.questList[i].goal.ItemCollected();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }
}
