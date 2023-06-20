using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class IdleState : MonsterBaseState<Spider>
    {
        bool isWaiting;

        public IdleState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isWaiting = false;
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (Vector3.Distance(owner.transform.position, owner.spawnPos) > 0.1f)
            {
                owner.StartCoroutine(returnPosRoutine());   
            }

            if (!isWaiting)
            {
                if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.biteAttackRange)
                {
                    owner.ChangeState(Spider.State.Attack);
                }
                else if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.detectRange)
                {
                    owner.ChangeState(Spider.State.Trace);
                }
            }
        }

        IEnumerator returnPosRoutine()
        {
            isWaiting = true;
            owner.animator.SetBool("Move", true);

            Vector3 start = owner.transform.position;
            Vector3 end = owner.spawnPos;

            Quaternion targetRot = Quaternion.LookRotation((end - start).normalized);
            owner.transform.rotation = targetRot;

            float totalTime = Vector3.Distance(start, end) / 5f;

            float rate = 0f;
            while (rate < 1)
            {
                rate += Time.deltaTime / totalTime;
                owner.transform.position = Vector3.Lerp(start, end, rate);
                yield return null;
            }

            owner.animator.SetBool("Move", false);
            isWaiting = false;
        }
    }
}