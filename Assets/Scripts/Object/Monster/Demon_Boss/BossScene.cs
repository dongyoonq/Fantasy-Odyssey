using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossScene : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera bossCam;
    [SerializeField] public CinemachineVirtualCamera endingCam;
    [SerializeField] public CinemachineFreeLook playerCam;
    [SerializeField] public CinemachineDollyCart dollyCart;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform playerBattlePoint;
    [SerializeField] Transform bossBattlePoint;
    [SerializeField] Transform endingPortalPoint;

    bool sceneActivity;

    private DemonBoss boss;

    private float shakeTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(BossSceneTrigger());
            GetComponent<Collider>().enabled = false;
        }
    }

    Canvas fadeCanvas;
    Animator fadeAnimator;

    IEnumerator BossSceneTrigger()
    {
        sceneActivity = true;
        //Player.Instance.controller.enabled = false;
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineFreeLook>();
        Player.Instance.GetComponent<PlayerInput>().enabled = false;

        fadeCanvas = GameManager.Resource.Instantiate<Canvas>("UI/FadeUI");
        fadeAnimator = fadeCanvas.GetComponent<Animator>();
        fadeAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(2f);
        bossCam.Priority = playerCam.Priority + 1;
        dollyCart.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        fadeAnimator.SetBool("FadeOut", false);
        fadeAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(1f);
        ShakeCamera(6f, 10f);
        StartCoroutine(CameraZoomRoutine(bossCam, 35f, 10f));
        yield return new WaitForSeconds(0.5f);
        fadeAnimator.SetBool("FadeIn", false);
        Destroy(fadeCanvas.gameObject);

        yield return new WaitForSeconds(4f);
        boss = GameManager.Resource.Instantiate<DemonBoss>("Prefabs/Monster/DemonBoss/demon_boss", spawnPoint.position, spawnPoint.rotation);
        boss.name = "데몬 보스";

        yield return new WaitForSeconds(1.6f);
        boss.enabled = false;

        yield return new WaitForSeconds(8f);
        fadeCanvas = GameManager.Resource.Instantiate<Canvas>("UI/FadeUI");
        fadeAnimator = fadeCanvas.GetComponent<Animator>();
        fadeAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(2f);
        Player.Instance.controller.enabled = false;
        boss.GetComponent<CharacterController>().enabled = false;
        Player.Instance.transform.position = playerBattlePoint.position;
        boss.transform.position = bossBattlePoint.position;
        bossCam.Priority -= 2;
        endingCam.LookAt = boss.transform;
        dollyCart.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        fadeAnimator.SetBool("FadeOut", false);
        fadeAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(1.5f);
        fadeAnimator.SetBool("FadeIn", false);
        Player.Instance.controller.enabled = true;
        boss.GetComponent<CharacterController>().enabled = true;
        Player.Instance.GetComponent<PlayerInput>().enabled = true;
        boss.enabled = true;
        Destroy(fadeCanvas.gameObject);
    }

    void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin channelPerlin = bossCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        channelPerlin.m_AmplitudeGain = intensity;

        shakeTimer = time;
    }

    bool onDied;

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0) 
            {
                CinemachineBasicMultiChannelPerlin channelPerlin = bossCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                channelPerlin.m_AmplitudeGain = 0f;
            }
        }

        if (boss.IsValid())
        {
            if (boss.currHp <= 0 && !onDied)
            {
                onDied = true;
                StartCoroutine(EndBossSceneRoutine());
            }
        }

        if (sceneActivity)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopAllCoroutines();

                if (!boss.IsValid())
                {
                    boss = GameManager.Resource.Instantiate<DemonBoss>("Prefabs/Monster/DemonBoss/demon_boss", spawnPoint.position, spawnPoint.rotation);
                    boss.name = "데몬 보스";
                }

                if (bossCam.Priority > playerCam.Priority)
                {
                    bossCam.Priority = playerCam.Priority - 1;
                }

                endingCam.LookAt = boss.transform;
                StartCoroutine(PositionSetting());
                Player.Instance.GetComponent<PlayerInput>().enabled = true;
                sceneActivity = false;
                dollyCart.gameObject.SetActive(false);
                if (fadeCanvas.IsValid())
                    Destroy(fadeCanvas.gameObject);
            }
        }
    }

    IEnumerator PositionSetting()
    {
        Player.Instance.controller.enabled = false;
        boss.GetComponent<CharacterController>().enabled = false;
        Player.Instance.transform.position = playerBattlePoint.position;
        boss.transform.position = bossBattlePoint.position;
        yield return new WaitForSeconds(0.1f);
        Player.Instance.controller.enabled = true;
        boss.GetComponent<CharacterController>().enabled = true;
    }

    IEnumerator EndBossSceneRoutine()
    {
        yield return null;
        boss.animator.SetFloat("DieSpeed", 0.3f);

        endingCam.transform.position = boss.transform.position + (boss.transform.up * 8f) + (boss.transform.right * 8f);
        endingCam.Priority = playerCam.Priority + 1;
        Time.timeScale = 0.8f;
        Player.Instance.GetComponent<PlayerInput>().enabled = false;

        StartCoroutine(CameraZoomRoutine(endingCam, 40f, 5f));
        yield return new WaitForSeconds(7f);

        Time.timeScale = 1f;
        playerCam.Priority = endingCam.Priority + 1;
        Player.Instance.GetComponent<PlayerInput>().enabled = true;

        GameObject portal = GameManager.Resource.Instantiate<GameObject>("Prefabs/Portal/DungeonToTown", endingPortalPoint.position, endingPortalPoint.rotation);
        portal.transform.localScale = new Vector3(41f, 41f, 41f);
    }

    IEnumerator CameraZoomRoutine(CinemachineVirtualCamera cam, float endFov, float duration)
    {
        float time = 0f;

        float startFov = cam.m_Lens.FieldOfView;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            bossCam.m_Lens.FieldOfView = Mathf.Lerp(startFov, endFov, t);

            yield return null;
        }
    }
}