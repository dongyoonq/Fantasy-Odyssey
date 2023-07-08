using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestUI : PopUpUI
{
    public GameObject questPanel;

    public bool activeQuest = false;

    Vector2 orgPosition;

    private void Start()
    {
        Player.Instance.questUI = this;

        if (questPanel.IsValid())
            questPanel.SetActive(activeQuest);

        questPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { OpenQuest(); GameManager.Sound.PlaySFX("Click"); });
        orgPosition = questPanel.transform.position;
    }

    public void OpenQuest(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Sound.PlaySFX("OpenUI");
            questPanel.transform.position = orgPosition;
            activeQuest = !activeQuest;
            questPanel.SetActive(activeQuest);
            questPanel.transform.parent.SetAsLastSibling();
        }
    }

    public void OpenQuest()
    {
        GameManager.Sound.PlaySFX("OpenUI");
        questPanel.transform.position = orgPosition;
        activeQuest = !activeQuest;
        questPanel.SetActive(activeQuest);
        questPanel.transform.parent.SetAsLastSibling();
    }
}
