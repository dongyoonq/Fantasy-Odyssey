using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    [NonSerialized] public QuestData quest;

    [NonSerialized] public GameObject questDescriptionPanel;
    GameObject questPanel;
    GameObject questContent;

    GameObject contentPrefab;
    GameObject instanceContent;

    TMP_Text titleText;
    TMP_Text descriptionText;
    TMP_Text rewardText;
    Button acceptButton;

    private void Start()
    {
        questPanel = GameObject.Find("QuestUI");
        questContent = questPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject;
        questDescriptionPanel = questPanel.transform.GetChild(1).gameObject;
        acceptButton = questDescriptionPanel.transform.GetChild(1).GetComponent<Button>();
        titleText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        descriptionText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        rewardText = questDescriptionPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        contentPrefab = GameManager.Resource.Load<GameObject>("UI/QuestSlot");
    }

    private void OnEnable()
    {
        quest = GetComponent<NPC>().data.quest;
    }

    public void OpenQuestfromNPC()
    {
        questDescriptionPanel.SetActive(true);
        titleText.text = quest.title;
        rewardText.text = $"\nReward : Exp {quest.expReward}";

        if (!quest.isActive)
        {
            descriptionText.text = quest.description;
            acceptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Accept";
            acceptButton.gameObject.SetActive(true);
        }
        else
        {
            if (!quest.goal.IsReached())
            {
                descriptionText.text = $"<color=#FFFF00>{name}</color>\n{quest.description}\n<color=#FF0000>퀘스트를 아직 완료하지 못했습니다.</color>\n<color=#04E2FD>{quest.target} : {quest.goal.currentAmount}/{quest.goal.requiredAmount}</color>";
                acceptButton.gameObject.SetActive(false);
            }
            else
            {
                descriptionText.text = $"<color=#FFFF00>{name}</color>\n{quest.description}\n<color=#00FF00>퀘스트를 완료했습니다.</color>";
                acceptButton.gameObject.SetActive(true);
                acceptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Ok";
            }
        }
    }

    public void OpenQuest()
    {
        questDescriptionPanel.SetActive(true);
        acceptButton.gameObject.SetActive(false);
        titleText.text = quest.title;
        rewardText.text = $"\nReward : Exp {quest.expReward}";

        if (!quest.isActive)
        {
            descriptionText.text = quest.description;
        }
        else
        {
            if (!quest.goal.IsReached())
            {
                descriptionText.text = $"<color=#FFFF00>{name}</color>\n{quest.description}\n<color=#FF0000>퀘스트를 아직 완료하지 못했습니다.</color>\n<color=#04E2FD>{quest.target} : {quest.goal.currentAmount}/{quest.goal.requiredAmount}</color>";
            }
            else
            {
                descriptionText.text = $"<color=#FFFF00>{name}</color>\n{quest.description}\n<color=#00FF00>퀘스트를 완료했습니다 해당 NPC에게 돌아가세요.</color>";
            }
        }
    }

    public void AcceptQuest()
    {
        if (!quest.isActive)
        {
            if (quest.goal.goalType == GoalType.Use)
            {
                quest.goal.currentAmount = 1;
                Player.Instance.AddItemToInventory(quest.goal.item);
            }

            questDescriptionPanel.SetActive(false);
            quest.isActive = true;
            Player.Instance.questList.Add(quest);

            instanceContent = Instantiate(contentPrefab, questContent.transform);
            instanceContent.transform.GetChild(0).GetComponent<Text>().text = quest.title;
            instanceContent.GetComponent<Button>().onClick.AddListener(OpenQuest);
            instanceContent.transform.GetChild(1).gameObject.SetActive(true); // Progress Active

            acceptButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void CompleteQuest()
    {
        if (quest.goal.IsReached())
        {
            Player.Instance.Exp += quest.expReward;
            questDescriptionPanel.SetActive(false);
            quest.isActive = false;
            Player.Instance.questList.Remove(quest);
            quest.goal.currentAmount = 0;
            Destroy(instanceContent);
            acceptButton.GetComponent<Button>().onClick.RemoveAllListeners();

            GameManager.Quest.questGivers.Remove(this);
        }
    }
}