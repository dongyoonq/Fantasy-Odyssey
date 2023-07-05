using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    [SerializeField] GameObject createPanel;
    [SerializeField] TMP_InputField inputText;
    [SerializeField] Button ok;
    [SerializeField] Button exit;
    [SerializeField] Button createOk;
    [SerializeField] Button createCancel;

    private void Start()
    {
        ok.onClick.AddListener(OpenCreate);
        exit.onClick.AddListener(Quit);
        createCancel.onClick.AddListener(CloseCreate);
        createOk.onClick.AddListener(CreatePlayer);
    }

    public void OpenCreate()
    {
        createPanel.SetActive(true);
    }

    public void CloseCreate()
    {
        createPanel.SetActive(false);
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
    }

    public void CreatePlayer()
    {
        if (string.IsNullOrEmpty(inputText.text))
            return;

        Player player = GameManager.Resource.Instantiate<Player>("Prefabs/Player/Player");
        player.PlayerName = inputText.text;

        GameManager.Scene.LoadScene("Town");

        createOk.onClick.RemoveAllListeners();
    }

    protected override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
