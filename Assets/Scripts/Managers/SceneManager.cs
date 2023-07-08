using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager; 

public class SceneManager : MonoBehaviour
{
    LoadingUI loadingUI;

    private BaseScene currScene;
    private List<string> gameTipList;

    private void Awake()
    {
        loadingUI = GameManager.Resource.Instantiate<LoadingUI>("UI/LoadingUI");
        loadingUI.transform.SetParent(transform);

        gameTipList = new List<string>
        {
            "게임 TIP : 필드아이템은 1초가 지나면 먹을수 있습니다.",
            "게임 TIP : 보스데몬의 공격패턴은 8가지가 됩니다.",
            "게임 TIP : 보스데몬은 기절치가 있어 쌓이게 되면 그로기 상태가 됩니다.",
            "게임 TIP : 무기는 종류마다 에니메이션과 이펙트가 다르게 나갑니다.",
            "게임 TIP : 필드몬스터 독침벌에 특정패턴을 맞게되면 독데미지를 입습니다.",
            "게임 TIP : 필드몬스터 팬텀은 주위를 돌아다니며 플레이어를 찾습니다.",
            "게임 TIP : 필드몬스터 독침벌은 소리를 감지하여 주위에서 뛰거나 걸으면 플레이어를 감지합니다. 주위에서 뛰지 않게 조심하세요.",
            "게임 TIP : 필드몬스터 독침벌은 시야 앞에 있게되면 플레이어를 감지합니다.",
            "게임 TIP : 필드몬스터 팬텀은 2타 공격을 하므로 두번 맞지않게 조심해야합니다.",
        };

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
        int randomValue = Random.Range(0, gameTipList.Count);

        GameManager.Sound.musicSource.Stop();
        GameManager.Sound.sfxSource.Stop();

        loadingUI.FadeOut();

        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;

        loadingUI.transform.GetChild(1).gameObject.SetActive(true);
        loadingUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = gameTipList[randomValue];
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