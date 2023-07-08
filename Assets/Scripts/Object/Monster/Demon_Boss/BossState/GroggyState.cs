using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class GroggyState : MonsterBaseState<DemonBoss>
    {
        public GroggyState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            GameManager.Sound.PlaySFX("Groggy");
            owner.groggyRoutine = owner.StartCoroutine(AnimationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.groggyRoutine);
        }

        public override void Update()
        {

        }

        IEnumerator AnimationRoutine()
        {
            owner.animator.SetBool("Groggy", true);
            yield return new WaitForSeconds(6f);
            owner.animator.SetBool("Groggy", false);
            owner.animator.SetBool("StandUp", true);
            yield return new WaitForSeconds(4.5f);
            owner.animator.SetBool("StandUp", false);
            owner.isGroggyed = false;
            owner.stunValue = 0;
            owner.ChangeState(DemonBoss.State.Idle);
        }
    }
}