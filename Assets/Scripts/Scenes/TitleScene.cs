using Cinemachine;
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
        GameManager.Sound.PlaySFX("Click");
        createPanel.SetActive(true);
    }

    public void CloseCreate()
    {
        GameManager.Sound.PlaySFX("Click");
        createPanel.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Sound.PlaySFX("Click");

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
        GameManager.Sound.PlaySFX("Click");

        if (string.IsNullOrEmpty(inputText.text))
            return;

        Player player = GameManager.Resource.Instantiate<Player>("Prefabs/Player/Player");
        player.PlayerName = inputText.text;

        GameManager.Scene.LoadScene("Town");

        Player.Instance.controller.enabled = false;
        Player.Instance.transform.position = new Vector3(216.596f, -24.09f, -84.016f);

        createOk.onClick.RemoveAllListeners();
    }

    protected override IEnumerator LoadingRoutine()
    {
        foreach (NpcData npc in GameManager.Quest.npcList)
        {
            if (npc.isQuestNPC)
            {
                npc.quest.goal.currentAmount = 0;
                npc.quest.goal.isCompleteTalked = false;
            }
        }

        yield return null;
        progress = 0.3f;

        GameManager.Sound.PlayMusic($"Title{UnityEngine.Random.Range(1, 4)}");
        GameManager.Sound.musicSource.loop = true;

        yield return null;
        progress = 0.6f;

        if (GameManager.Quest.IsValid())
            Destroy(GameManager.Quest.gameObject);

        yield return null;
        progress = 1f;

        GameManager.Sound.PlayMusic("Title2");
        GameManager.Sound.musicSource.loop = true;
    }
}
