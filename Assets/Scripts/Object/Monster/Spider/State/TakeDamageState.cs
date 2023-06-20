using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class TakeDamageState : MonsterBaseState<Spider>
    {
        private bool isCoroutineRunning;

        public TakeDamageState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("TakeDamage", true);
            isCoroutineRunning = true;
            owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(animationRoutine());
            owner.animator.SetBool("TakeDamage", false);
            isCoroutineRunning = false;
        }

        public override void Update()
        {
        }

        IEnumerator animationRoutine()
        {
            yield return new WaitForSeconds(0.6f);

            if (!isCoroutineRunning)
                yield break;

            owner.ChangeState(Spider.State.Idle);
        }
    }
}
