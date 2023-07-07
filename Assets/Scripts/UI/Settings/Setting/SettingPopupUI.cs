using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopupUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["Button_Close"].onClick.AddListener(() => { base.CloseUI(); });
        buttons["TitleButton"].onClick.AddListener(() => { StartCoroutine(ReturnTitle()); });
        buttons["ExitButton"].onClick.AddListener(() => { ExitGame(); });
    }


    IEnumerator ReturnTitle()
    {
        GameManager.Scene.LoadScene("Title");

        yield return new WaitForSeconds(0.5f);

        DestroyUI();
        GameManager.Resource.Destroy(Player.Instance.gameObject);
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
    }

    void DestroyUI()
    {
        if (GameObject.Find("ShopUI").IsValid())
        {
            GameManager.Resource.Destroy(GameObject.Find("ShopUI"));
        }

        if (GameObject.Find("PopupUI").IsValid())
        {
            GameManager.Resource.Destroy(GameObject.Find("PopupUI"));
        }

        if (GameObject.Find("SceneUI").IsValid())
        {
            GameManager.Resource.Destroy(GameObject.Find("SceneUI"));
        }
    }
}