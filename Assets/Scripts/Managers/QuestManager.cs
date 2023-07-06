using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [NonSerialized] public GameObject questDescriptionPanel;

    GameObject questPanel;
    GameObject contentArea;
    GameObject questListUI;

    GameObject contentPrefab;
    GameObject instanceContent;

    TMP_Text titleText;
    TMP_Text descriptionText;
    TMP_Text rewardText;
    Button acceptButton;

    private void Start()
    {
        questPanel = GameObject.Find("QuestUI");
        questListUI = questPanel.transform.GetChild(0).gameObject;
        contentArea = questListUI.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;

        questDescriptionPanel = questPanel.transform.GetChild(1).gameObject;
        acceptButton = questDescriptionPanel.transform.GetChild(1).GetComponent<Button>();
        titleText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        descriptionText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        rewardText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        contentPrefab = GameManager.Resource.Load<GameObject>("UI/QuestSlot");

        Player.Instance.OnChangeKillQuestUpdate.AddListener(UpdateKillQuest);
        Player.Instance.OnChangeGatheringQuestUpdate.AddListener(UpdateGatheringQuest);
        Player.Instance.OnChangeUseQuestUpdate.AddListener(UpdateUseQuest);
        Player.Instance.OnChangeTalkQuestUpdate.AddListener(UpdateTalkQuest);
    }

    void UpdateKillQuest(BaseMonsterData monsterData)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Kill QuestData
            if (Player.Instance.questList[i].questData.questName == "SpiderKill" 
                && monsterData.id == "Spider" && Player.Instance.questList[i].questData.goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].questData.goal.EnemyKilled();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // Attack Tutorial Kill QuestData
            if (Player.Instance.questList[i].questData.questName == "AttackTutorial"
                && monsterData.id == "ScareCrow" && Player.Instance.questList[i].questData.goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].questData.goal.EnemyKilled();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // StingBee Kill QuestData
            if (Player.Instance.questList[i].questData.questName == "StingBee"
                && monsterData.id == "StingBee" && Player.Instance.questList[i].questData.goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].questData.goal.EnemyKilled();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // Phantom Kill QuestData
            if (Player.Instance.questList[i].questData.questName == "Phantom"
                && monsterData.id == "Phantom" && Player.Instance.questList[i].questData.goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].questData.goal.EnemyKilled();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // DemonBoss QuestData
            if (Player.Instance.questList[i].questData.questName == "DemonBoss"
                && monsterData.id == "DemonBoss" && Player.Instance.questList[i].questData.goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].questData.goal.EnemyKilled();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }

    void UpdateGatheringQuest(ItemData item)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Gathering QuestData
            if (Player.Instance.questList[i].questData.questName == "SpiderGathering" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Gathering && item.itemName == "SpiderBooty")
            {
                Player.Instance.questList[i].questData.goal.ItemCollected();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // StingVenom Gathering QuestData
            if (Player.Instance.questList[i].questData.questName == "StingVenom" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Gathering && item.itemName == "Venomous Sting")
            {
                Player.Instance.questList[i].questData.goal.ItemCollected();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // PhantomEye Gathering QuestData
            if (Player.Instance.questList[i].questData.questName == "PhantomEye" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Gathering && item.itemName == "Spectral Eye")
            {
                Player.Instance.questList[i].questData.goal.ItemCollected();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // DemonBossHeart Gathering QuestData
            if (Player.Instance.questList[i].questData.questName == "DemonBossHeart" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Gathering && item.itemName == "Demon's Heart")
            {
                Player.Instance.questList[i].questData.goal.ItemCollected();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }

    void UpdateUseQuest(ItemData item)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Gathering QuestData
            if (Player.Instance.questList[i].questData.questName == "UsePotion" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Use && item.itemName == "Hp Potion")
            {
                Player.Instance.questList[i].questData.goal.ItemUse();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }

    void UpdateTalkQuest(NpcData npc)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Noa Quest
            if (Player.Instance.questList[i].questData.questName == "TalkNoa" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == npc)
            {
                Player.Instance.questList[i].questData.goal.TalkNpc();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Rayleigh Quest
            if (Player.Instance.questList[i].questData.questName == "TalkRayleigh" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == npc)
            {
                Player.Instance.questList[i].questData.goal.TalkNpc();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Alice Quest
            if (Player.Instance.questList[i].questData.questName == "TalkAlice" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == npc)
            {
                Player.Instance.questList[i].questData.goal.TalkNpc();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Cassius Quest
            if (Player.Instance.questList[i].questData.questName == "TalkCassius" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == npc)
            {
                Player.Instance.questList[i].questData.goal.TalkNpc();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk King Quest
            if (Player.Instance.questList[i].questData.questName == "TalkKing" &&
                Player.Instance.questList[i].questData.goal.goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == npc)
            {
                Player.Instance.questList[i].questData.goal.TalkNpc();

                if (Player.Instance.questList[i].questData.goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }


    public void OpenQuestfromNPC(QuestData quest)
    {
        questDescriptionPanel.SetActive(true);
        titleText.text = quest.title;
        rewardText.text = $"\nReward : Exp {quest.expReward}";

        if (!CheckPlayerQuest(quest))
        {
            descriptionText.text = quest.description;
            acceptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Accept";
            acceptButton.gameObject.SetActive(true);
        }
        else
        {
            if (!quest.goal.IsReached())
            {
                descriptionText.text = $"{quest.description}\n<color=#FF0000>퀘스트를 아직 완료하지 못했습니다.</color>\n<color=#04E2FD>{quest.target} : {quest.goal.currentAmount}/{quest.goal.requiredAmount}</color>";
                acceptButton.gameObject.SetActive(false);
            }
            else
            {
                descriptionText.text = $"{quest.description}\n<color=#00FF00>퀘스트를 완료했습니다.</color>";
                acceptButton.gameObject.SetActive(true);
                acceptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Ok";
            }
        }
    }

    public void OpenQuest(QuestData quest)
    {
        questDescriptionPanel.SetActive(true);
        acceptButton.gameObject.SetActive(false);
        titleText.text = quest.title;
        rewardText.text = $"\nReward : Exp {quest.expReward}";

        if (!CheckPlayerQuest(quest))
        {
            descriptionText.text = quest.description;
        }
        else
        {
            if (!quest.goal.IsReached())
            {
                descriptionText.text = $"{quest.description}\n<color=#FF0000>퀘스트를 아직 완료하지 못했습니다.</color>\n<color=#04E2FD>{quest.target} : {quest.goal.currentAmount}/{quest.goal.requiredAmount}</color>";
            }
            else
            {
                descriptionText.text = $"{quest.description}\n<color=#00FF00>퀘스트를 완료했습니다 해당 NPC에게 돌아가세요.</color>";
            }
        }
    }

    public void AcceptQuest(QuestData quest)
    {
        if (quest.goal.goalType == GoalType.Use)
        {
            quest.goal.currentAmount = 1;
            Player.Instance.AddItemToInventory(quest.goal.item);
        }

        Quest newQuest = new Quest();

        questDescriptionPanel.SetActive(false);
        newQuest.questData = quest;

        Player.Instance.questList.Add(newQuest);

        instanceContent = Instantiate(contentPrefab, contentArea.transform);
        instanceContent.transform.GetChild(0).GetComponent<Text>().text = quest.title;
        instanceContent.GetComponent<Button>().onClick.AddListener(() => OpenQuest(quest));
        instanceContent.transform.GetChild(1).gameObject.SetActive(true); // Progress Active

        acceptButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void CompleteQuest(QuestData quest)
    {
        if (quest.goal.IsReached())
        {
            questDescriptionPanel.SetActive(false);
            Player.Instance.Exp += quest.expReward;

            RemoveQuest(quest);

            Destroy(instanceContent);
            acceptButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    void RemoveQuest(QuestData data)
    {
        foreach (Quest quest in Player.Instance.questList)
        {
            if (quest.questData == data)
            {
                Player.Instance.questList.Remove(quest);
                Player.Instance.completeQuest.Add(quest);
                Debug.Log(quest.questData + "clear");
                return;
            }
        }
    }

    bool CheckPlayerQuest(QuestData data)
    {
        foreach (Quest quest in Player.Instance.questList)
        {
            if (quest.questData == data)
            {
                return true;
            }
        }

        return false;
    }
}
