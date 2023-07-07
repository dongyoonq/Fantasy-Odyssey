using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopupUI : PopUpUI
{
    Slider bgmSlider;
    Slider sfxSlider;

    protected override void Awake()
    {
        base.Awake();

        buttons["Button_Close"].onClick.AddListener(() => { base.CloseUI(); });
        buttons["TitleButton"].onClick.AddListener(() => { StartCoroutine(ReturnTitle()); });
        buttons["ExitButton"].onClick.AddListener(() => { ExitGame(); });
        buttons["BgmButton"].onClick.AddListener(() =>
        {
            GameManager.Sound.ToggleMusic();
            if (GameManager.Sound.musicSource.mute)
                buttons["BgmButton"].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 0, 1);
            else
                buttons["BgmButton"].transform.GetChild(0).GetComponent<Image>().color = new Color(0.5294118f, 0.4705882f, 0.3843137f, 1);
        });
        buttons["SfxButton"].onClick.AddListener(() =>
        {
            GameManager.Sound.ToggleSFX();
            if (GameManager.Sound.sfxSource.mute)
                buttons["SfxButton"].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 0, 1);
            else
                buttons["SfxButton"].transform.GetChild(0).GetComponent<Image>().color = new Color(0.5294118f, 0.4705882f, 0.3843137f, 1);
        });
        bgmSlider = transforms["SliderBgm"].GetComponent<Slider>();
        sfxSlider = transforms["SliderSfx"].GetComponent<Slider>();
    }

    private void Update()
    {
        GameManager.Sound.MusicVolume(bgmSlider.value);
        GameManager.Sound.SFXVolume(sfxSlider.value);
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