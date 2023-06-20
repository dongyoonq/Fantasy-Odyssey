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
            owner.animator.SetBool("TakeDamage", true);
            owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("TakeDamage", false);
        }

        public override void Update()
        {

        }

        IEnumerator animationRoutine()
        {
            yield return new WaitForSeconds(0.8f);
            owner.ChangeState(Spider.State.Trace);
        }
    }
}