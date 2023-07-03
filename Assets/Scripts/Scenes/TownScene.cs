using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        progress = 0.2f;
        CinemachineFreeLook frCam = GameManager.Resource.Instantiate<CinemachineFreeLook>("Prefabs/Player/PlayerCam");
        frCam.LookAt = Player.Instance.transform;
        frCam.Follow = Player.Instance.transform;
        Player.Instance.GetComponent<MouseController>().FrCam = frCam;
        yield return null;

        progress = 0.5f;
        GameManager.Resource.Instantiate<Canvas>("UI/ShopUI");
        GameManager.Resource.Instantiate<Canvas>("UI/PopupUI");
        GameManager.Resource.Instantiate<Canvas>("UI/SceneUI");
        yield return null;

        progress = 0.7f;
        GameObject questObj = new GameObject();
        questObj.name = "QuestManager";
        questObj.AddComponent<QuestManager>();
        yield return null;

        progress = 0.9f;
        Player.Instance.transform.position = transform.position;
        yield return null;

        Player.Instance.controller.enabled = true;

        progress = 1f;
    }
}
