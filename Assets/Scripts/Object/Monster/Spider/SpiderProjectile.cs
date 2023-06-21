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
            // moveRoutine�� ���߿� ��Ų �ð��� ���� üũ�� �Ͽ� �׽ð� ��ŭ Wait�ϱ� ���� ���
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
