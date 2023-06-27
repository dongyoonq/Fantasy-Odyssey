using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    GameObject talkPanel;

    TMP_Text npcNameText;
    TMP_Text npcContentText;
    Button okButton;

    QuestGiver questNpc;
    bool isCompleteQuest;
    bool isQuestNPC;

    [SerializeField, TextArea] List<string> talkContents;

    private void Start()
    {
        talkPanel = GameObject.Find("TalkUI").transform.GetChild(0).gameObject;
        npcNameText = talkPanel.transform.GetChild(0).GetComponent<TMP_Text>();
        npcContentText = talkPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        okButton = talkPanel.transform.GetChild(2).GetComponent<Button>();

        if (TryGetComponent(out questNpc))
            isQuestNPC = true;
        else
            isQuestNPC= false;

        isCompleteQuest = false;
    }

    public void OpenTalk()
    {
        talkPanel.SetActive(true);
        okButton.onClick.RemoveAllListeners();
        npcNameText.text = name;

        if (isQuestNPC)
        {
            questNpc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

            if (!questNpc.quest.isActive)
            {
                if (isCompleteQuest)
                {
                    npcContentText.text = talkContents[0];
                    okButton.onClick.AddListener(() => {
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });
                    return;
                }

                npcContentText.text = talkContents[1];

                okButton.onClick.AddListener(() => {
                    questNpc.OpenQuestfromNPC();
                    npcNameText.transform.parent.gameObject.SetActive(false);
                });

                questNpc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => questNpc.AcceptQuest());
            }
            else
            {
                if (questNpc.quest.goal.IsReached())
                {
                    isCompleteQuest = true;
                    npcContentText.text = talkContents[3];
                    questNpc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => questNpc.CompleteQuest());
                    okButton.onClick.AddListener(() => {
                        questNpc.OpenQuestfromNPC();
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });
                    return;
                }

                npcContentText.text = talkContents[2];

                okButton.onClick.AddListener(() => {
                    npcNameText.transform.parent.gameObject.SetActive(false);
                });
            }
        }
        else
        {
            npcContentText.text = talkContents[0];
            okButton.onClick.AddListener(() => {
                npcNameText.transform.parent.gameObject.SetActive(false);
            });
        }
    }
}
