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
    TMP_Text npcInGameName;
    Button okButton;

    public Animator animator;

    [SerializeField] public NpcData data;

    private void Start()
    {
        StartCoroutine(SetPanel());
        animator = GetComponent<Animator>();
    }

    IEnumerator SetPanel()
    {
        yield return new WaitForSeconds(0.1f);
        talkPanel = GameObject.Find("TalkUI").transform.GetChild(0).gameObject;
        npcNameText = talkPanel.transform.GetChild(0).GetComponent<TMP_Text>();
        npcContentText = talkPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        okButton = talkPanel.transform.GetChild(2).GetComponent<Button>();
        npcInGameName = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        npcInGameName.text = name;

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
        animator.SetBool("Talk", true);
        talkPanel.SetActive(true);
        okButton.onClick.RemoveAllListeners();
        npcNameText.text = name;

        QuestData giver;

        if (CheckTargetNpc(out giver))
        {
            QuestGiver npc = GameObject.Find(giver.name).GetComponent<QuestGiver>();

            npc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

            npcContentText.text = data.talkData.talkContents[4];

            okButton.onClick.AddListener(() => {
                npc.OpenQuestfromNPC();
                npcNameText.transform.parent.gameObject.SetActive(false);
            });

            npc.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => npc.CompleteQuest());

            return;
        }

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

    public void FinishTalkNpc()
    {
        animator.SetBool("Talk", false);
    }

    public bool CheckTargetNpc(out QuestData giver)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            if (Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == this.data)
            {
                giver = Player.Instance.questList[i];
                Player.Instance.OnChangeTalkQuestUpdate?.Invoke(this.data);
                return true;
            }
        }

        giver = null;
        return false;
    }
}
