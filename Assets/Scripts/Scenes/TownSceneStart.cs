using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TownSceneStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineFreeLook frCam = GameManager.Resource.Instantiate<CinemachineFreeLook>("Prefabs/Player/PlayerCam");
        frCam.LookAt = Player.Instance.transform;
        frCam.Follow = Player.Instance.transform;
        Player.Instance.GetComponent<MouseController>().FrCam = frCam;

        GameObject questObj = new GameObject();
        questObj.name = "QuestManager";
        questObj.AddComponent<QuestManager>();

        Player.Instance.transform.position = transform.position;
    }
}
