using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Demon_Boss
{
    public class JumpAttackState : MonsterBaseState<DemonBoss>
    {
        ParticleSystem particle;
        CinemachineImpulseSource impulseSource;

        public JumpAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            impulseSource = owner.GetComponent<CinemachineImpulseSource>();
            owner.animator.SetFloat("MoveSpeed", 0);
            owner.jumpAttackRoutine = owner.StartCoroutine(JumpAttackRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.jumpAttackRoutine);
            if (!owner.pharse2)
                owner.coolTime = 1.5f;
            else
                owner.coolTime = 1f;
            owner.patternChangeTimer = 0f;

            if (particle.IsValid())
                GameManager.Resource.Destroy(particle.gameObject, 1f);
        }

        public override void Update()
        {

        }

        IEnumerator JumpAttackRoutine()
        {
            owner.animator.SetBool("Jump", true);

            float time = 0f;

            while (time < 1f)
            {
                time += Time.deltaTime;

                owner.ySpeed = Mathf.Lerp(owner.ySpeed, 6.5f, time);

                yield return null;
            }

            yield return new WaitForSeconds(0.6f);
            impulseSource.GenerateImpulse();
            AttackJudgeMent();
            particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Monster/DemonBoss/GroundCrack", owner.transform.position + (owner.transform.up * 0.2f), Quaternion.identity);
            GameManager.Resource.Destroy(particle.gameObject, 1f);
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