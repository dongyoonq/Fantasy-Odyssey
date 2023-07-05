using System.Collections;
using UnityEngine;

namespace PhantomState
{
    public class DamageState : MonsterBaseState<Phantom>
    {
        public DamageState(Phantom owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Damage", true);
            owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(animationRoutine());
            owner.animator.SetBool("Damage", false);
        }

        public override void Update()
        {
        }

        IEnumerator animationRoutine()
        {
            yield return new WaitForSeconds(0.6f);
            owner.ChangeState(Phantom.State.Idle);
        }
    }
}
