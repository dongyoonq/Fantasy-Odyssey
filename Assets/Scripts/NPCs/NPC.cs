using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    GameObject talkPanel;

    TMP_Text npcNameText;
    TMP_Text npcContentText;
    Button okButton;

    [SerializeField] public NpcData data;

    private void Start()
    {
        talkPanel = GameObject.Find("TalkUI").transform.GetChild(0).gameObject;
        npcNameText = talkPanel.transform.GetChild(0).GetComponent<TMP_Text>();
        npcContentText = talkPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        okButton = talkPanel.transform.GetChild(2).GetComponent<Button>();

        Debug.Log(data);

        if (data.isQuestNPC)
        {
            data.questNpc = transform.AddComponent<QuestGiver>();
            data.quest.goal.currentAmount = 0;
            data.quest.isActive = false;
        }

        data.isCompleteQuest = false;
    }

    public void OpenTalk()
    {
        talkPanel.SetActive(true);
        okButton.onClick.RemoveAllListeners();
        npcNameText.text = name;

        if (data.isQuestNPC)
        {
            data.questNpc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

            if (!data.quest.isActive)
            {
                if (data.isCompleteQuest)
                {
                    npcContentText.text = data.talkData.talkContents[0];
                    okButton.onClick.AddListener(() => {
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });
                    return;
                }

                npcContentText.text = data.talkData.talkContents[1];

                okButton.onClick.AddListener(() => {
                    data.questNpc.OpenQuestfromNPC();
                    npcNameText.transform.parent.gameObject.SetActive(false);
                });

                data.questNpc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => data.questNpc.AcceptQuest());
            }
            else
            {
                if (data.quest.goal.IsReached())
                {
                    data.isCompleteQuest = true;
                    npcContentText.text = data.talkData.talkContents[3];
                    data.questNpc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => data.questNpc.CompleteQuest());
                    okButton.onClick.AddListener(() => {
                        data.questNpc.OpenQuestfromNPC();
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });
                    return;
                }

                npcContentText.text = data.talkData.talkContents[2];

                okButton.onClick.AddListener(() => {
                    npcNameText.transform.parent.gameObject.SetActive(false);
                });
            }
        }
        else
        {
            npcContentText.text = data.talkData.talkContents[0];
            okButton.onClick.AddListener(() => {
                npcNameText.transform.parent.gameObject.SetActive(false);
            });
        }
    }
}
