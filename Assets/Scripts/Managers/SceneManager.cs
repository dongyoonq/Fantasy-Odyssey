using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager; 

public class SceneManager : MonoBehaviour
{
    LoadingUI loadingUI;

    private BaseScene currScene;

    private void Awake()
    {
        loadingUI = GameManager.Resource.Instantiate<LoadingUI>("UI/LoadingUI");
        loadingUI.transform.SetParent(transform);
    }

    public BaseScene CurrScene
    {
        get
        {
            if (currScene == null)
                currScene = GameObject.FindObjectOfType<BaseScene>();

            return currScene;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        GameManager.Sound.musicSource.Stop();
        GameManager.Sound.sfxSource.Stop();

        loadingUI.FadeOut();

        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;

        loadingUI.transform.GetChild(1).gameObject.SetActive(true);
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);

        while (!oper.isDone)
        {
            loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress));
            yield return null;
        }

        CurrScene.LoadAsync();

        while (CurrScene.progress < 1f)
        {
            if (CurrScene.progress == 0.5f)
            {
                GameManager.Pool.Restart();
            }

            loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, currScene.progress));
            yield return null;
        }

        Time.timeScale = 1f;
        loadingUI.transform.GetChild(1).gameObject.SetActive(false);
        loadingUI.FadeIn();
        yield return new WaitForSeconds(0.5f);
    }
}