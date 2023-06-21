using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class TakeDamageState : MonsterBaseState<Spider>
    {
        public TakeDamageState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.takedamageRoutine = owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.takedamageRoutine);
            owner.animator.SetBool("TakeDamage", false);
        }

        public override void Update()
        {
        }

        IEnumerator animationRoutine()
        {
            owner.animator.SetBool("TakeDamage", true);
            yield return new WaitForSeconds(0.6f);

            owner.ChangeState(Spider.State.Idle);
        }
    }
}
