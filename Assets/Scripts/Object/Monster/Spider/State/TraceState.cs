using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SpiderState
{
    public class TraceState : MonsterBaseState<Spider>
    {
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
            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            if (owner.controller.enabled)
                owner.controller.Move(targetDir * owner.data.moveSpeed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

            if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.data.meleeMonsterData[0].detectRange)
            {
                owner.ChangeState(Spider.State.BiteAttack);
            }
            else if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.data.rangeMonsterData[0].detectRange)
            {
                owner.ChangeState(Spider.State.ProjecTileAttack);
            }
            else if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) > owner.data.agressiveMonsterData[0].detectRange)
            {
                owner.ChangeState(Spider.State.Return);
            }
        }
    }
}