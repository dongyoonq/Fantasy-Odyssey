using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [NonSerialized] public GameObject questDescriptionPanel;

    [SerializeField] public List<NpcData> npcList;

    GameObject questPanel;
    GameObject contentArea;
    GameObject questListUI;

    QuestSlot contentPrefab;
    List<QuestSlot> questSlots;

    TMP_Text titleText;
    TMP_Text descriptionText;
    TMP_Text rewardText;
    Button acceptButton;

    readonly Dictionary<string, QuestCheckData> questTable = new Dictionary<string, QuestCheckData>();

    const string attackTutorial = "AttackTutorial";
    const string spiderKill = "SpiderKill";
    const string spiderGathering = "SpiderGather";
    const string stingBeeKill = "StingBeeKill";
    const string stingBeeGathering = "StingBeeGather";
    const string phantomKill = "PhantomKill";
    const string phantomGahtering = "PhantomGather";
    const string bossKill = "BossKill";
    const string bossGathering = "BossGather";
    const string potionTutorial = "PotionTutorial";
    const string talkNoa = "TalkNoa";
    const string talkRayleigh = "TalkRayleigh";
    const string talkNox = "TalkNox";   
    const string talkCassius = "TalkCassius";
    const string talkKing = "TalkKing";

    private void Awake()
    {
        questTable.Add(attackTutorial, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/이솔"));
        questTable.Add(spiderKill, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/녹스"));
        questTable.Add(stingBeeKill, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/마리엘"));
        questTable.Add(phantomKill, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/카시우스"));
        questTable.Add(bossKill, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/고딘"));
        questTable.Add(spiderGathering, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/진"));
        questTable.Add(stingBeeGathering, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/올리비아"));
        questTable.Add(phantomGahtering, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/이자벨"));
        questTable.Add(bossGathering, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/아델라인"));
        questTable.Add(potionTutorial, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/한스"));
        questTable.Add(talkNoa, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/토마스"));
        questTable.Add(talkRayleigh, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/노아"));
        questTable.Add(talkNox, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/레일리"));
        questTable.Add(talkCassius, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/슌"));
        questTable.Add(talkKing, Resources.Load<QuestCheckData>("Data/NpcData/QuestCheck/제스터"));

    }

    private void Start()
    {
        questSlots = new List<QuestSlot>();
        npcList = new List<NpcData>();
        questPanel = GameObject.Find("QuestUI");
        questListUI = questPanel.transform.GetChild(0).gameObject;
        contentArea = questListUI.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;

        questDescriptionPanel = questPanel.transform.GetChild(1).gameObject;
        acceptButton = questDescriptionPanel.transform.GetChild(1).GetComponent<Button>();
        titleText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        descriptionText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        rewardText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        contentPrefab = GameManager.Resource.Load<QuestSlot>("UI/QuestSlot");

        Player.Instance.OnChangeKillQuestUpdate.AddListener(UpdateKillQuest);
        Player.Instance.OnChangeGatheringQuestUpdate.AddListener(UpdateGatheringQuest);
        Player.Instance.OnChangeUseQuestUpdate.AddListener(UpdateUseQuest);
        Player.Instance.OnChangeTalkQuestUpdate.AddListener(UpdateTalkQuest);
        questDescriptionPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            Player.Instance.playerInput.enabled = true;
            Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;
        });
    }

    void UpdateKillQuest(BaseMonsterData monsterData)
    {
        UpdateQuest(GoalType.Kill, monsterData);
    }

    void UpdateGatheringQuest(ItemData item)
    {
        UpdateQuest(GoalType.Gathering, item);
    }

    void UpdateUseQuest(ItemData item)
    {
        UpdateQuest(GoalType.Use, item);
    }

    void UpdateTalkQuest(NpcData npc)
    {
        UpdateQuest(GoalType.Talk, npc);
    }

    private void UpdateQuest(GoalType goalType, ScriptableObject checkData)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            foreach (QuestCheckData questCheckData in questTable.Values)
            {
                if (questCheckData.questData != Player.Instance.questList[i].questData)
                    continue;

                if (goalType == GoalType.Kill && questCheckData.targetMonster == checkData)
                {
                    QuestContentUpdate(i);
                    Debug.Log("킬 호출");
                    return;
                }
                else if (goalType == GoalType.Gathering && questCheckData.targetItem == checkData)
                {
                    QuestContentUpdate(i);
                    return;
                }
                else if (goalType == GoalType.Use && Player.Instance.questList[i].questData.goal.item == checkData)
                {
                    QuestContentUpdate(i);
                    return;
                }
                else if (goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == checkData)
                {
                    QuestContentUpdate(i);
                    return;
                }
            }
        }
    }

    private void QuestContentUpdate(int index)
    {
        switch (Player.Instance.questList[index].questData.goal.goalType)
        {
            case GoalType.Kill:
                Player.Instance.questList[index].questData.goal.EnemyKilled(); break;
            case GoalType.Gathering:
                Player.Instance.questList[index].questData.goal.ItemCollected(); break;
            case GoalType.Use:
                Player.Instance.questList[index].questData.goal.ItemUse(); break;
            case GoalType.Talk:
                Player.Instance.questList[index].questData.goal.TalkNpc(); break;
        }

        if (Player.Instance.questList[index].questData.goal.IsReached())
        {
            contentArea.transform.GetChild(index).GetChild(1).gameObject.SetActive(false);  // progress
            contentArea.transform.GetChild(index).GetChild(2).gameObject.SetActive(true);   // complete
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
        Player.Instance.playerInput.enabled = true;
        Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;

        if (quest.goal.goalType == GoalType.Use)
        {
            quest.goal.currentAmount = 1;
            Player.Instance.AddItemToInventory(quest.goal.item);
        }

        Quest newQuest = new Quest();

        questDescriptionPanel.SetActive(false);
        newQuest.questData = quest;

        Player.Instance.questList.Add(newQuest);

        QuestSlot instanceContent = Instantiate(contentPrefab, contentArea.transform);
        instanceContent.transform.GetChild(0).GetComponent<Text>().text = quest.title;
        instanceContent.GetComponent<Button>().onClick.AddListener(() => { OpenQuest(quest); GameManager.Sound.PlaySFX("Click"); });
        instanceContent.transform.GetChild(1).gameObject.SetActive(true); // Progress Active
        instanceContent.questData = quest;
        questSlots.Add(instanceContent);

        acceptButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void CompleteQuest(QuestData quest)
    {
        Player.Instance.playerInput.enabled = true;
        Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;

        if (quest.goal.IsReached())
        {
            questDescriptionPanel.SetActive(false);
            Player.Instance.Exp += quest.expReward;

            RemoveQuest(quest);

            Destroy(questSlots.Find(x => x.questData == quest).gameObject);
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
