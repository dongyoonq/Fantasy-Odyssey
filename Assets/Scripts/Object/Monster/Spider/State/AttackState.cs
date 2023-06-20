using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SpiderState
{
    public class AttackState : MonsterBaseState<Spider>
    {
        public AttackState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.coolTime = 0.8f;
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
                owner.transform.position = Vector3.Lerp(start, end, rate);
                yield return null;
            }

            owner.animator.SetBool("Move", false);
            owner.animator.SetBool("Attack", true);
            // 공격 코드를 추가합니다.

            yield return new WaitForSeconds(1f); // animation Timing
            owner.ChangeState(Spider.State.Trace);
        }
    }
}