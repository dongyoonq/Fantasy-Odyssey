using System.Collections;
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
        }

        public override void Update()
        {
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), 6 * Time.deltaTime);
        }

        IEnumerator JabAttackRoutine()
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
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);
                yield return null;
            }

            owner.animator.SetBool("Move", false);
            owner.animator.SetBool("Attack1", true);

            yield return new WaitForSeconds(0.5f);
            attackJudgement();
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

            end += hit.normal * 1.2f;

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
            attackJudgement();
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(StingBee.State.Trace);
        }

        IEnumerator StingAttackRoutine()
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
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);
                yield return null;
            }

            owner.animator.SetBool("Move", false);
            owner.animator.SetBool("Attack3", true);

            yield return new WaitForSeconds(0.5f);
            attackJudgement();
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(StingBee.State.Trace);
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