using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderProjectile : MonoBehaviour
{
    public Spider spider;
    float elapseTime;
    public Coroutine coroutine;

    private void OnEnable()
    {
        elapseTime = 0f;
    }

    private void Update()
    {
        elapseTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // moveRoutine중 도중에 끊킨 시간에 대해 체크를 하여 그시간 만큼 Wait하기 위해 계산
            float calculateTime = spider.ProjecttileTime - elapseTime;
            other.GetComponent<IHitable>().Hit(spider.data.RangeMonsterData[0].AttackDamage);

            coroutine = spider.StartCoroutine(waitTime(calculateTime));

            if (gameObject.IsValid())
                GameManager.Resouce.Destroy(gameObject);
        }
    }

    IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        spider.ChangeState(Spider.State.Trace);
    }
}
