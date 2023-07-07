using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        progress = 0.2f;
        CinemachineFreeLook frCam = GameManager.Resource.Instantiate<CinemachineFreeLook>("Prefabs/Player/PlayerCam");
        frCam.name = "PlayerCam";
        frCam.LookAt = Player.Instance.transform;
        frCam.Follow = Player.Instance.transform;
        Player.Instance.GetComponent<MouseController>().FrCam = frCam;
        yield return null;

        progress = 0.5f;
        ReLoadUi();
        yield return null;

        progress = 0.7f;
        ReLoadQuestMgr();
        yield return null;

        progress = 0.9f;
        //Player.Instance.transform.position = transform.position;
        yield return null;

        Player.Instance.controller.enabled = true;
        Player.Instance.GetComponent<PlayerInput>().enabled = true;
        if (Player.Instance.noticeUI.noticePanel.IsValid())
            Player.Instance.noticeUI.noticePanel.gameObject.SetActive(false);

        progress = 1f;
    }

    void ReLoadUi()
    {
        GameManager.Ui.Restart();

        if (!GameObject.Find("ShopUI").IsValid())
        {
            Canvas shopUi = GameManager.Resource.Instantiate<Canvas>("UI/ShopUI");
            shopUi.name = "ShopUI";
            shopUi.transform.SetParent(GameManager.Ui.transform);
        }

        if (!GameObject.Find("PopupUI").IsValid())
        {
            Canvas popupUi = GameManager.Resource.Instantiate<Canvas>("UI/PopupUI");
            popupUi.name = "PopupUI";
            popupUi.transform.SetParent(GameManager.Ui.transform);
        }

        if (!GameObject.Find("SceneUI").IsValid())
        {
            Canvas sceneUi = GameManager.Resource.Instantiate<Canvas>("UI/SceneUI");
            sceneUi.name = "SceneUI";
            sceneUi.transform.SetParent(GameManager.Ui.transform);
        }
    }

    void ReLoadQuestMgr()
    {
        if (!GameObject.Find("QuestManager").IsValid())
        {
            GameObject questObj = new GameObject();
            questObj.name = "QuestManager";
            questObj.transform.parent = GameManager.Instance.transform;
            questObj.AddComponent<QuestManager>();
            GameManager.Quest = questObj.GetComponent<QuestManager>();
        }
    }
}