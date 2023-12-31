using System.Collections;
using System.Net;
using UnityEngine;

namespace Demon_Boss
{
    public class ClawAttackState : MonsterBaseState<DemonBoss>
    {
        int randomValue;

        public ClawAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            randomValue = Random.Range(1, 3);
            if (!owner.pharse2)
                owner.clawRoutine = owner.StartCoroutine(AnimationRoutine());
            else
                owner.clawRoutine = owner.StartCoroutine(Pharse2animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.clawRoutine);
            owner.animator.SetBool($"Claw{randomValue}", false);
            if (!owner.pharse2)
                owner.coolTime = 1.3f;
            else
                owner.coolTime = 0.8f;
        }

        public override void Update()
        {
            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }

        IEnumerator AnimationRoutine()
        {
            owner.animator.SetFloat("MoveSpeed", owner.data.moveSpeed);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 3f;

            float totalTime = Vector3.Distance(start, end) / owner.data.moveSpeed;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;

                Vector3 targetPosition = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);

                // Calculate the movement direction and speed
                Vector3 moveDirection = (targetPosition - owner.transform.position).normalized;

                // Move the character using the Character Controller
                owner.controller.Move(moveDirection * owner.data.moveSpeed * Time.deltaTime);

                if (rate > 0.8f)
                    owner.animator.SetBool($"Claw{randomValue}", true);

                yield return null;
            }

            GameManager.Sound.PlaySFX($"Claw{randomValue}");

            // animation Timing
            yield return new WaitForSeconds(0.3f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, 360f);
            yield return new WaitForSeconds(0.7f);
            owner.animator.SetFloat("MoveSpeed", 0);
            owner.ChangeState(DemonBoss.State.Move);
        }

        IEnumerator Pharse2animationRoutine()
        {
            owner.animator.SetFloat("MoveSpeed", owner.data.moveSpeed);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 3f;

            float totalTime = Vector3.Distance(start, end) / owner.data.moveSpeed;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);

                if (rate > 0.8f)
                    owner.animator.SetBool($"Claw1", true);

                yield return null;
            }

            GameManager.Sound.PlaySFX("Claw1");

            // animation Timing
            yield return new WaitForSeconds(0.3f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, 360f);
            yield return new WaitForSeconds(0.3f);
            owner.animator.SetBool($"Claw1", false);
            GameManager.Sound.PlaySFX("Claw2");
            owner.animator.SetBool($"Claw2", true);
            yield return new WaitForSeconds(0.8f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, 360f);
            yield return new WaitForSeconds(0.5f);
            owner.animator.SetBool($"Claw2", false);
            owner.animator.SetFloat("MoveSpeed", 0);
            owner.ChangeState(DemonBoss.State.Move);
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