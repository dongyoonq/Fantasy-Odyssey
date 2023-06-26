using UnityEngine;
using UnityEngine.InputSystem;

public class QuestUI : PopUpUI
{
    public GameObject questPanel;
    public GameObject questContent;

    [SerializeField] Sprite questInProgressIcon;
    [SerializeField] Sprite questInComplete;

    public bool activeQuest = false;

    private void Start()
    {
        if (questPanel.IsValid())
            questPanel.SetActive(activeQuest);
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
}
