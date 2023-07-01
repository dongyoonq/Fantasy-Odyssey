using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    public DemonBoss owner;
    float elapseTime;

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
            float calculateTime = owner.rockElapseTime - elapseTime;
            other.GetComponent<IHitable>().Hit(owner.data.rangeMonsterData[0].attackDamage);

            owner.StopCoroutine(owner.rockAttackRoutine);
            owner.rockAttackRoutine = null;
            owner.StartCoroutine(waitTime(calculateTime));

            if (gameObject.IsValid())
                GameManager.Resource.Destroy(gameObject);
        }
    }

    IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        owner.ChangeState(DemonBoss.State.Move);
    }
}
