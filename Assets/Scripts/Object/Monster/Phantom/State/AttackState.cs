using System.Collections;
using UnityEngine;

namespace PhantomState
{
    public class AttackState : MonsterBaseState<Phantom>
    {
        public AttackState(Phantom owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.StartCoroutine(AttackRoutine());
        }

        public override void Exit()
        {
            owner.coolTime = 1f;
            owner.StopCoroutine(AttackRoutine());
        }

        public override void Update()
        {

        }

        IEnumerator AttackRoutine()
        {
            owner.animator.SetBool("Move", true);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 1.5f;

            float totalTime = Vector3.Distance(start, end) / owner.data.moveSpeed;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;

                Vector3 targetPosition = Vector3.Lerp(start, end, rate);

                // Calculate the movement direction and speed
                Vector3 moveDirection = (targetPosition - owner.transform.position).normalized;

                // Move the character using the Character Controller
                owner.controller.Move(moveDirection * owner.data.moveSpeed * Time.deltaTime);

                if (rate > 0.7f)
                    owner.animator.SetBool("AttackLeft", true);

                yield return null;
            }

            // animation Timing
            yield return new WaitForSeconds(0.3f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, 360f);
            yield return new WaitForSeconds(0.3f);
            owner.animator.SetBool("AttackLeft", false);
            owner.animator.SetBool("AttackRight", true);
            yield return new WaitForSeconds(0.3f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, 360f);
            yield return new WaitForSeconds(0.3f);
            owner.animator.SetBool("AttackRight", false);
            owner.animator.SetBool("Move", false);
            owner.ChangeState(Phantom.State.Trace);
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