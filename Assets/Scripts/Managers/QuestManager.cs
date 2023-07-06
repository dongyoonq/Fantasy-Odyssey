using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<QuestGiver> questGivers = new List<QuestGiver>();

    GameObject contentArea;
    GameObject questListUI;

    private void Start()
    {
        questListUI = GameObject.Find("QuestUI").transform.GetChild(0).gameObject;
        contentArea = questListUI.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
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
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "SpiderKill" 
                && monsterData.id == "Spider" && Player.Instance.questList[i].goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].goal.EnemyKilled();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // Attack Tutorial Kill QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "AttackTutorial"
                && monsterData.id == "ScareCrow" && Player.Instance.questList[i].goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].goal.EnemyKilled();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // StingBee Kill QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "StingBee"
                && monsterData.id == "StingBee" && Player.Instance.questList[i].goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].goal.EnemyKilled();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // Phantom Kill QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "Phantom"
                && monsterData.id == "Phantom" && Player.Instance.questList[i].goal.goalType == GoalType.Kill)
            {
                Player.Instance.questList[i].goal.EnemyKilled();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // DemonBoss QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "DemonBoss"
                && monsterData.id == "DemonBoss" && Player.Instance.questList[i].goal.goalType == GoalType.Kill)
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
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Gathering QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "SpiderGathering" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Gathering && item.itemName == "SpiderBooty")
            {
                Player.Instance.questList[i].goal.ItemCollected();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // StingVenom Gathering QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "StingVenom" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Gathering && item.itemName == "Venomous Sting")
            {
                Player.Instance.questList[i].goal.ItemCollected();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // PhantomEye Gathering QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "PhantomEye" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Gathering && item.itemName == "Spectral Eye")
            {
                Player.Instance.questList[i].goal.ItemCollected();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }

            // DemonBossHeart Gathering QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "DemonBossHeart" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Gathering && item.itemName == "Demon's Heart")
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

    void UpdateUseQuest(ItemData item)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Spider Gathering QuestData
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "UsePotion" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Use && item.itemName == "Hp Potion")
            {
                Player.Instance.questList[i].goal.ItemUse();

                if (Player.Instance.questList[i].goal.IsReached())
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
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "TalkNoa" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == npc)
            {
                Player.Instance.questList[i].goal.TalkNpc();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Rayleigh Quest
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "TalkRayleigh" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == npc)
            {
                Player.Instance.questList[i].goal.TalkNpc();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Alice Quest
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "TalkAlice" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == npc)
            {
                Player.Instance.questList[i].goal.TalkNpc();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk Cassius Quest
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "TalkCassius" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == npc)
            {
                Player.Instance.questList[i].goal.TalkNpc();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }

        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            // Talk King Quest
            if (Player.Instance.questList[i].isActive && Player.Instance.questList[i].questName == "TalkKing" &&
                Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == npc)
            {
                Player.Instance.questList[i].goal.TalkNpc();

                if (Player.Instance.questList[i].goal.IsReached())
                {
                    contentArea.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);  // progress
                    contentArea.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);   // complete
                }
            }
        }
    }
}
