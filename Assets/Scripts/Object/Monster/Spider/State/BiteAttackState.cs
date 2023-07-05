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

                Vector3 targetPosition = Vector3.Lerp(start, end, rate);

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
            attackJudgement();
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(Spider.State.Trace);
        }


        void attackJudgement()
        {
            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, owner.data.meleeMonsterData[0].attackDistance, LayerMask.GetMask("Player")))
            {
                hit.collider.GetComponent<IHitable>().Hit(owner.data.meleeMonsterData[0].attackDamage);
            }

        }
    }
}