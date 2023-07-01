using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class JumpAttackState : MonsterBaseState<DemonBoss>
    {
        ParticleSystem particle;

        public JumpAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetFloat("MoveSpeed", 0);
            owner.jumpAttackRoutine = owner.StartCoroutine(JumpAttackRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.jumpAttackRoutine);
            owner.coolTime = 1f;
            owner.patternChangeTimer = 0f;
        }

        public override void Update()
        {

        }

        IEnumerator JumpAttackRoutine()
        {
            owner.animator.SetBool("Jump", true);
            yield return new WaitForSeconds(1.2f);
            AttackJudgeMent();
            particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Monster/DemonBoss/GroundCrack", owner.transform.position + (owner.transform.up * 0.2f), Quaternion.identity);
            GameManager.Resource.Destroy(particle, 1f);
            yield return new WaitForSeconds(0.3f);
            owner.animator.SetBool("Jump", false);
            owner.ChangeState(DemonBoss.State.Move);
        }

        void AttackJudgeMent()
        {
            if (Player.Instance.playerController.isGrounded)
            {
                Player.Instance.Hit(1000);
            }
        }
    }
}