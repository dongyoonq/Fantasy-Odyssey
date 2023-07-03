using System.Collections;
using UnityEngine;

namespace StingBeeState
{
    public class TakeDamageState : MonsterBaseState<StingBee>
    {
        public TakeDamageState(StingBee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.takedamageRoutine = owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.takedamageRoutine);
            owner.animator.SetBool("Damage", false);
        }

        public override void Update()
        {
        }

        IEnumerator animationRoutine()
        {
            owner.animator.SetBool("Damage", true);
            yield return new WaitForSeconds(0.6f);

            owner.ChangeState(StingBee.State.Idle);
        }
    }
}
