using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public GameObject questDescriptionPanel;
    public GameObject questContent;
    GameObject contentPrefab;
    GameObject questInProgressIcon;
    GameObject instanceContent;

    public Text titleText;
    public Text descriptionText;
    public Text rewardText;
    public GameObject acceptButton;

    private void Start()
    {
        contentPrefab = GameManager.Resource.Load<GameObject>("UI/QuestSlot");
        questInProgressIcon = GameManager.Resource.Load<GameObject>("UI/QuestInProgress");
    }

    public void OpenQuestfromNPC()
    {
        questDescriptionPanel.SetActive(true);
        titleText.text = quest.title;
        rewardText.text = $"Reward : Exp {quest.expReward}";

        if (!quest.isActive)
        {
            descriptionText.text = quest.description;
            acceptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Accept";
            acceptButton.SetActive(true);
        }
        else
        {
            if (!quest.goal.IsReached())
            {
                descriptionText.text = $"{quest.description}\n<color=#FF0000>아직완료되지 않았습니다.</color>";
                acceptButton.SetActive(false);
            }
            else
            {
                descriptionText.text = $"{quest.description}\n<color=#FFFF00>퀘스트를 완료했습니다 확인을 누르면 완료됩니다.</color>";
                acceptButton.SetActive(true);
                acceptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Ok";
            }
        }
    }

    public void OpenQuest()
    {
        questDescriptionPanel.SetActive(true);
        acceptButton.SetActive(false);
        titleText.text = quest.title;
        rewardText.text = $"Reward : Exp {quest.expReward}";

        if (!quest.isActive)
        {
            descriptionText.text = quest.description;
        }
        else
        {
            if (!quest.goal.IsReached())
            {
                descriptionText.text = $"{quest.description}\n<color=#FF0000>아직완료되지 않았습니다.</color>";
            }
            else
            {
                descriptionText.text = $"{quest.description}\n<color=#FFFF00>퀘스트를 완료했습니다 해당 NPC에게 돌아가십쇼</color>";
            }
        }
    }

    public void AcceptQuest()
    {
        if (!quest.isActive)
        {
            questDescriptionPanel.SetActive(false);
            quest.isActive = true;
            Player.Instance.questList.Add(quest);

            instanceContent = Instantiate(contentPrefab, questContent.transform);
            instanceContent.transform.GetChild(0).GetComponent<Text>().text = quest.title;
            instanceContent.GetComponent<Button>().onClick.AddListener(OpenQuest);
            instanceContent.transform.GetChild(1).gameObject.SetActive(true); // Progress Active

            acceptButton.GetComponent<Button>().onClick.RemoveAllListeners();
            Player.Instance.isAddedQuestListener = false;
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
            Player.Instance.isAddedQuestListener = false;
        }
    }
}