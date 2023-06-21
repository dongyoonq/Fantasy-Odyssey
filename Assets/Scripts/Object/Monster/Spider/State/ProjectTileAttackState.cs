using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class ProjectTileAttackState : MonsterBaseState<Spider>
    {
        float projectileSpeed = 3.5f;
        ParticleSystem particle;

        public ProjectTileAttackState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.ProjecttileTime = 0;
            owner.animator.SetBool("ProjectileAttack", true);
            owner.projectTileAttackRoutine = owner.StartCoroutine(attackRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("ProjectileAttack", false);
            owner.StopCoroutine(owner.projectTileAttackRoutine);
            owner.StopCoroutine(owner.projectTileMoveRoutine);
            if (particle.IsValid())
                GameManager.Resouce.Destroy(particle.gameObject);
        }

        public override void Update()
        {
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), 6 * Time.deltaTime);
        }

        IEnumerator attackRoutine()
        {
            yield return new WaitForSeconds(0.4f);
            particle = GameManager.Resouce.Instantiate<ParticleSystem>("Prefabs/Monster/Spider/SpiderProjectileAttack",
                owner.transform.position + (owner.transform.forward * 0.8f) + (owner.transform.up * 0.3f), owner.transform.rotation, true);
            particle.GetComponent<SpiderProjectile>().spider = owner;
            owner.projectTileMoveRoutine = owner.StartCoroutine(projectileMoveRoutine(particle));
        }

        IEnumerator projectileMoveRoutine(ParticleSystem particle)
        {
            Vector3 start = particle.transform.position;
            Vector3 end = start + (particle.transform.forward * owner.data.RangeMonsterData[0].AttackDistance);
            float totalTime = Vector3.Distance(start, end) / projectileSpeed;
            owner.ProjecttileTime = totalTime;

            float rate = 0;

            if (particle.IsValid())
            {
                while (rate < 1)
                {
                    rate += Time.deltaTime / totalTime;
                    particle.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);
                    yield return null;
                }

                if (particle.IsValid())
                    GameManager.Resouce.Destroy(particle.gameObject);
                owner.ChangeState(Spider.State.Trace);
            }
        }
    }
}