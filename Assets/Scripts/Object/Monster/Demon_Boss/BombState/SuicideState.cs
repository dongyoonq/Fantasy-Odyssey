using System.Collections;
using UnityEngine;

namespace Demon_Bomb
{
    public class SuicideState : MonsterBaseState<DemonBomb>
    {
        bool isSuicided;

        public SuicideState(DemonBomb owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isSuicided = false;
            owner.animator.SetBool("Move", true);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (!isSuicided)
            {
                Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

                Quaternion targetRot = Quaternion.LookRotation(TargetDir);
                owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

                owner.transform.Translate(new Vector3(TargetDir.x, 0, TargetDir.z) * owner.data.moveSpeed * Time.deltaTime, Space.World);

                if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < 2.5f)
                {
                    owner.animator.SetBool("Move", false);
                    isSuicided = true;
                    owner.ChangeState(DemonBomb.State.Suicide);
                    owner.StartCoroutine(AnimationRoutine());
                }
            }
        }

        IEnumerator AnimationRoutine()
        {
            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            float totalTime = Vector3.Distance(start, end) / owner.data.moveSpeed;
            float rate = 0f;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 3f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);

                if (rate > 0.2f)
                    owner.animator.SetBool($"Suicide", true);

                yield return null;
            }

            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, owner.data.meleeMonsterData[0].angle);
            owner.animator.SetBool("Suicide", false);
            GameManager.Resource.Destroy(owner.gameObject);
            ParticleSystem particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Monster/DemonBoss/BombExplosion", owner.transform.position + (owner.transform.up * 1f), Quaternion.identity);
            GameManager.Resource.Destroy(particle.gameObject, 1f);
        }

        void AttackCircleJudgement(int damage, float range, float forwardAngle, float upAngle)
        {
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position, range, LayerMask.GetMask("Player"));
            foreach (Collider collider in colliders)
            {
                Vector3 dirTarget = (collider.transform.position - owner.transform.position).normalized;

                if (Vector3.Dot(owner.transform.up, dirTarget) >= Mathf.Cos(upAngle * 0.5f * Mathf.Deg2Rad) &&
                    Vector2.Dot(owner.transform.forward, dirTarget) >= Mathf.Cos(forwardAngle * 0.5f * Mathf.Deg2Rad))
                    collider.GetComponent<IHitable>().Hit(damage);
            }
        }
    }
}