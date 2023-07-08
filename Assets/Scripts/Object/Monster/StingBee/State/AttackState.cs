using System.Collections;
using System.Dynamic;
using UnityEngine;

namespace StingBeeState
{
    public class AttackState : MonsterBaseState<StingBee>
    {
        int randomValue;

        public AttackState(StingBee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            randomValue = Random.Range(1, 4);

            if (randomValue == 1)
                owner.attackRoutine = owner.StartCoroutine(JabAttackRoutine());
            else if (randomValue == 2)
                owner.attackRoutine = owner.StartCoroutine(PowerJabAttackRoutine());
            else if (randomValue == 3)
                owner.attackRoutine = owner.StartCoroutine(StingAttackRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.attackRoutine);
            if (randomValue == 1)
                owner.animator.SetBool("Attack1", false);
            else if (randomValue == 2)
                owner.animator.SetBool("Attack2", false);
            else if (randomValue == 3)
                owner.animator.SetBool("Attack3", false);

            owner.coolTime = 0.8f;
        }

        public override void Update()
        {
            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), 6 * Time.deltaTime);
        }

        IEnumerator JabAttackRoutine()
        {
            owner.animator.SetBool("Move", true);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 1.8f;

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
            owner.animator.SetBool("Attack1", true);
            GameManager.Sound.PlaySFX("BeeAttack");

            yield return new WaitForSeconds(0.5f);
            AttackCircleJudgement(owner.data.meleeMonsterData[0].attackDamage, owner.data.meleeMonsterData[0].attackDistance, owner.data.meleeMonsterData[0].angle, 330f);
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(StingBee.State.Trace);
        }

        IEnumerator PowerJabAttackRoutine()
        {
            owner.animator.SetBool("Move", true);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 1.8f;

            float totalTime = Vector3.Distance(start, end) / 5f;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);
                yield return null;
            }

            owner.animator.SetBool("Move", false);
            owner.animator.SetBool("Attack2", true);
            yield return new WaitForSeconds(0.5f);
            GameManager.Sound.PlaySFX("BeeAttack");
            yield return new WaitForSeconds(0.4f);
            AttackCircleJudgement(owner.data.meleeMonsterData[1].attackDamage, owner.data.meleeMonsterData[1].attackDistance, owner.data.meleeMonsterData[1].angle, 330f);
            yield return new WaitForSeconds(0.4f);
            owner.ChangeState(StingBee.State.Trace);
        }

        IEnumerator StingAttackRoutine()
        {
            owner.animator.SetBool("Move", true);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 1.8f;

            float totalTime = Vector3.Distance(start, end) / 5f;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);
                yield return null;
            }

            owner.animator.SetBool("Move", false);
            owner.animator.SetBool("Attack3", true);
            GameManager.Sound.PlaySFX("BeeAttack");

            yield return new WaitForSeconds(0.5f);
            AttackCircleJudgement(owner.data.meleeMonsterData[2].attackDamage, owner.data.meleeMonsterData[2].attackDistance, owner.data.meleeMonsterData[2].angle, 330f);
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(StingBee.State.Trace);
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
                    if (randomValue == 3)
                        owner.StartCoroutine(PoisionDamageRoutine(damage));
                    else
                        collider.GetComponent<IHitable>().Hit(damage);
                }
            }
        }

        IEnumerator PoisionDamageRoutine(int damage)
        {
            int count = 0;

            while (count < 4)
            {
                Player.Instance.Hit(damage);
                count++;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}