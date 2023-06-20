using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SpiderState
{
    public class TraceState : MonsterBaseState<Spider>
    {
        bool isWaiting;
        float moveSpeed = 5f;
        float rotSpeed = 6f;

        public TraceState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Move", true);
        }

        public override void Exit()
        {
            owner.animator.SetBool("Move", false);
        }

        public override void Update()
        {
            if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.biteAttackRange)
            {
                owner.ChangeState(Spider.State.Attack);
            }
            else if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) > owner.detectRange)
            {
                owner.ChangeState(Spider.State.Idle);
            }

            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            owner.transform.Translate(new Vector3(TargetDir.x, 0, TargetDir.z) * moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), rotSpeed * Time.deltaTime);
        }
    }
}