using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestUI : PopUpUI
{
    public GameObject questPanel;

    public bool activeQuest = false;

    private void Start()
    {
        Player.Instance.questUI = this;

        if (questPanel.IsValid())
            questPanel.SetActive(activeQuest);

        questPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { OpenQuest(); });
    }

    public void OpenQuest(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            activeQuest = !activeQuest;
            GameManager.Ui.activePopupUI = activeQuest;
            questPanel.SetActive(activeQuest);
        }
    }

    public void OpenQuest()
    {
        activeQuest = !activeQuest;
        GameManager.Ui.activePopupUI = activeQuest;
        questPanel.SetActive(activeQuest);
    }
}
