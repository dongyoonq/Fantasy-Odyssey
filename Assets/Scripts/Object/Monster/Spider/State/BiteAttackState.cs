using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class BiteAttackState : MonsterBaseState<Spider>
    {
        public BiteAttackState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.biteRoutine = owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.biteRoutine);
            owner.animator.SetBool("Attack", false);
        }

        public override void Update()
        {
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), 6 * Time.deltaTime);
        }

        IEnumerator animationRoutine()
        {
            owner.animator.SetBool("Move", true);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 1.2f;

            float totalTime = Vector3.Distance(start, end) / 5f;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;

                Vector3 targetPosition = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);

                // Calculate the movement direction and speed
                Vector3 moveDirection = (targetPosition - owner.transform.position).normalized;

                // Move the character using the Character Controller
                owner.controller.Move(moveDirection * owner.data.moveSpeed * Time.deltaTime);

                yield return null;
            }

            owner.animator.SetBool("Move", false);
            owner.animator.SetBool("Attack", true);

            // Wait for the specified animation timing
            yield return new WaitForSeconds(0.5f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance + 0.5f, 180f, 320f);
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(Spider.State.Trace);
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