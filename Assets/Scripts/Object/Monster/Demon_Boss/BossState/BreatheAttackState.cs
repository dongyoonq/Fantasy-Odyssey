using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class BreatheAttackState : MonsterBaseState<DemonBoss>
    {
        ParticleSystem particle;
        bool isWait;
        bool playerHited;

        public BreatheAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isWait = true;
            playerHited = false;
            owner.animator.SetBool("Breath", true);
            owner.breatheRoutine = owner.StartCoroutine(AnimationRoutine());
            owner.hitBoxRoutine = owner.StartCoroutine(HitBoxOn());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Breath", false);
            if (owner.clawRoutine != null)
                owner.StopCoroutine(owner.clawRoutine);
            if (owner.hitBoxRoutine != null)
                owner.StopCoroutine(owner.hitBoxRoutine);
            if (particle.IsValid())
                GameManager.Resource.Destroy(particle);
            owner.StopCoroutine(HitBoxOn());
            if (!owner.pharse2)
                owner.coolTime = 1.5f;
            else
                owner.coolTime = 1f;
            owner.patternChangeTimer = 0f;
        }

        public override void Update()
        {
            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        
            if (!isWait && !playerHited)
            {
                AttackCircleJudgement(250, 6, 180, 360);
            }
        }

        IEnumerator AnimationRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Monster/DemonBoss/Flame", owner.jaw.transform.position, owner.jaw.transform.rotation, owner.jaw.transform);
            yield return new WaitForSeconds(3f);
            owner.ChangeState(DemonBoss.State.Idle);
            if (particle.IsValid())
                GameManager.Resource.Destroy(particle);
        }

        IEnumerator HitBoxOn()
        {
            yield return new WaitForSeconds(1.5f);
            isWait = false;
        }

        void AttackCircleJudgement(int damage, float range, float forwardAngle, float upAngle)
        {
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position, range, LayerMask.GetMask("Player"));
            foreach (Collider collider in colliders)
            {
                Vector3 dirTarget = (collider.transform.position - owner.transform.position).normalized;

                if (Vector3.Dot(owner.transform.up, dirTarget) >= Mathf.Cos(upAngle * 0.5f * Mathf.Deg2Rad) &&
                    Vector2.Dot(owner.transform.forward, dirTarget) >= Mathf.Cos(forwardAngle * 0.5f * Mathf.Deg2Rad))
                {
                    collider.GetComponent<IHitable>().Hit(damage);
                    playerHited = true;
                    owner.StartCoroutine(reHitBoxOn());
                }
            }
        }

        IEnumerator reHitBoxOn()
        {
            yield return new WaitForSeconds(0.5f);
            playerHited = false;
        }
    }
}