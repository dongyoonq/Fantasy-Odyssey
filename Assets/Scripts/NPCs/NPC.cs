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
                GameManager.Sound.PlaySFX("Click");
                transform.GetComponent<ShopNpc>().OpenShop();
                npcNameText.transform.parent.gameObject.SetActive(false);
            });

            return;
        }

        QuestData questData;

        if (CheckTargetNpc(out questData))
        {
            GameManager.Quest.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

            npcContentText.text = data.talkData.talkContents[4];

            okButton.onClick.AddListener(() => {
                GameManager.Sound.PlaySFX("Click");
                GameManager.Quest.OpenQuestfromNPC(questData);
                npcNameText.transform.parent.gameObject.SetActive(false);
            });

            GameManager.Quest.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { GameManager.Quest.CompleteQuest(questData); GameManager.Sound.PlaySFX("Click"); });

            return;
        }

        if (data.isQuestNPC)
        {
            if (data.isTargetNpc)
            {
                if (!CheckPrevComplete())
                {
                    npcContentText.text = data.talkData.talkContents[0];
                    okButton.onClick.AddListener(() => {
                        GameManager.Sound.PlaySFX("Click");
                        npcNameText.transform.parent.gameObject.SetActive(false);
                        Player.Instance.playerInput.enabled = true;
                        Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;
                    });
                    return;
                }
            }

            GameManager.Quest.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

            Quest playerQuest;

            // 플레이어에 이 퀘스트가 없다면
            if (!CheckPlayerQuest(out playerQuest))
            {
                // 완료된 퀘스트인지 확인
                if (CheckCompleteQuest())
                {
                    // 이미 완료된 퀘스트
                    npcContentText.text = data.talkData.talkContents[0];
                    okButton.onClick.AddListener(() =>
                    {
                        GameManager.Sound.PlaySFX("Click");
                        npcNameText.transform.parent.gameObject.SetActive(false);
                        Player.Instance.playerInput.enabled = true;
                        Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;
                    });
                    return;
                }
                else
                {
                    // 수락, 처음시작 하는 퀘스트
                    npcContentText.text = data.talkData.talkContents[1];

                    okButton.onClick.AddListener(() => {
                        GameManager.Sound.PlaySFX("Click");
                        GameManager.Quest.OpenQuestfromNPC(data.quest);
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });

                    GameManager.Quest.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => 
                    {
                        if (!GameManager.Quest.npcList.Contains(data))
                            GameManager.Quest.npcList.Add(data);
                        GameManager.Quest.AcceptQuest(data.quest); 
                        GameManager.Sound.PlaySFX("Click"); 
                    });
                }
            }
            // 플레이어에 이 퀘스트가 있으면
            else
            {
                // 달성조건 확인
                if (playerQuest.questData.goal.IsReached())
                {
                    // 달성시
                    npcContentText.text = data.talkData.talkContents[3];
                    GameManager.Quest.questDescriptionPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => 
                    { 
                        GameManager.Quest.CompleteQuest(data.quest); GameManager.
                        Sound.PlaySFX("Click"); 
                    });

                    okButton.onClick.AddListener(() => {
                        GameManager.Sound.PlaySFX("Click");
                        GameManager.Quest.OpenQuestfromNPC(data.quest);
                        npcNameText.transform.parent.gameObject.SetActive(false);
                    });
                    return;
                }
                else
                {
                    // 미달성시
                    npcContentText.text = data.talkData.talkContents[2];

                    okButton.onClick.AddListener(() => {
                        GameManager.Sound.PlaySFX("Click");
                        npcNameText.transform.parent.gameObject.SetActive(false);
                        Player.Instance.playerInput.enabled = true;
                        Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;
                    });
                }
            }
        }
        else
        {
            npcContentText.text = data.talkData.talkContents[0];
            okButton.onClick.AddListener(() => {
                GameManager.Sound.PlaySFX("Click");
                npcNameText.transform.parent.gameObject.SetActive(false);
                Player.Instance.playerInput.enabled = true;
                Player.Instance.mouseController.mouseSensitivity = Player.Instance.mouseController.prevMousSens;
            });
        }
    }

    public void FinishTalkNpc()
    {
        animator.SetBool("Talk", false);
    }

    public bool CheckTargetNpc(out QuestData questData)
    {
        for (int i = 0; i < Player.Instance.questList.Count; i++)
        {
            if (Player.Instance.questList[i].questData.goal.goalType == GoalType.Talk && Player.Instance.questList[i].questData.goal.targetNpc == this.data)
            {
                questData = Player.Instance.questList[i].questData;
                Player.Instance.OnChangeTalkQuestUpdate?.Invoke(this.data);
                return true;
            }
        }

        questData = null;
        return false;
    }

    public bool CheckPrevComplete()
    {
        for (int i = 0; i < Player.Instance.completeQuest.Count; i++)
        {
            if (Player.Instance.completeQuest[i].questData.goal.goalType == GoalType.Talk && Player.Instance.completeQuest[i].questData.goal.targetNpc == this.data)
            {
                return true;
            }
        }

        return false;
    }

    bool CheckPlayerQuest(out Quest playerQuest)
    {
        foreach (Quest quest in Player.Instance.questList)
        {
            if (quest.questData == data.quest)
            {
                playerQuest = quest;
                return true;
            }
        }

        playerQuest = null;
        return false;
    }

    bool CheckCompleteQuest()
    {
        foreach (Quest quest in Player.Instance.completeQuest)
        {
            if (quest.questData == data.quest)
            {
                return true;
            }
        }

        return false;
    }
}
