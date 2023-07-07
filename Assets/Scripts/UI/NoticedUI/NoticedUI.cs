using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NoticedUI : PopUpUI
{
    public GameObject noticePanel;

    public bool activeNotice = false;

    private Transform contentArea;
    private Button button1;
    private Button button2;
    private Button button3;

    private void Start()
    {
        Player.Instance.noticeUI = this;
        contentArea = noticePanel.transform.GetChild(2).GetChild(0).GetChild(0);
        button1 = contentArea.GetChild(0).GetComponent<Button>();
        button2 = contentArea.GetChild(1).GetComponent<Button>();
        button3 = contentArea.GetChild(2).GetComponent<Button>();

        if (noticePanel.IsValid())
            noticePanel.SetActive(activeNotice);

        Player.Instance.OnDied.AddListener(() => StartCoroutine(OpenNotice()));
        button1.onClick.AddListener(() => StartCoroutine(SpawnTown()));
        button2.onClick.AddListener(() => StartCoroutine(ReturnTitle()));
        button3.onClick.AddListener(ExitGame);
    }

    IEnumerator OpenNotice()
    {
        yield return new WaitForSeconds(3f);
        activeNotice = true;
        noticePanel.SetActive(activeNotice);
        noticePanel.transform.parent.SetAsLastSibling();
    }

    void CloseNotice()
    {
        activeNotice = false;
        noticePanel.SetActive(activeNotice);
    }

    IEnumerator SpawnTown()
    {
        GameManager.Scene.LoadScene("Town");

        yield return new WaitForSeconds(0.5f);

        Player.Instance.stateMachine.onDied = false;
        Player.Instance.controller.enabled = false;
        Player.Instance.transform.position = new Vector3(216.596f, -24.09f, -84.016f);
        Player.Instance.CurrentHP = Player.Instance.Status.MaxHp;

        yield return new WaitForSeconds(0.01f);

        CloseNotice();

        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    IEnumerator ReturnTitle()
    {
        CloseNotice();

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
