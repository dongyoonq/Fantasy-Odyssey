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
    [SerializeField] public bool isShopNpc;

    bool isPrevQuestComplete;

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

        if (isShopNpc)
        {
            npcContentText.text = data.talkData.talkContents[0];

            okButton.onClick.AddListener(() => {
                transform.GetComponent<ShopNpc>().OpenShop();
                npcNameText.transform.parent.gameObject.SetActive(false);
            });

            return;
        }

        QuestGiver giver;

        if (CheckTargetNpc(out giver))
        {
            giver.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

            npcContentText.text = data.talkData.talkContents[4];

            okButton.onClick.AddListener(() => {
                giver.OpenQuestfromNPC();
                npcNameText.transform.parent.gameObject.SetActive(false);
            });

            giver.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => giver.CompleteQuest());

            isPrevQuestComplete = true;

            return;
        }

        if (data.isQuestNPC)
        {
            if (data.isTargetNpc)
            {
                if (!isPrevQuestComplete)
                {
                    npcContentText.text = data.talkData.talkContents[0];
                    okButton.onClick.AddListener(() => {
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });
                    return;
                }
            }

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

    public bool CheckTargetNpc(out QuestGiver giver)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            if (Player.Instance.questList[i].goal.goalType == GoalType.Talk && Player.Instance.questList[i].goal.targetNpc == this.data)
            {
                foreach (QuestGiver givers in GameManager.Quest.questGivers)
                {
                    if (givers.quest == Player.Instance.questList[i])
                    {
                        giver = givers;
                        Player.Instance.OnChangeTalkQuestUpdate?.Invoke(this.data);
                        return true;
                    }
                }
            }
        }

        giver = null;
        return false;
    }
}
