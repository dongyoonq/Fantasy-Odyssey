using System.Collections;
using UnityEngine;

namespace Demon_Bomb
{
    public class MoveState : MonsterBaseState<DemonBomb>
    {
        public MoveState(DemonBomb owner) : base(owner)
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
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

            owner.transform.Translate(new Vector3(TargetDir.x, 0, TargetDir.z) * owner.data.moveSpeed * Time.deltaTime, Space.World);

            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.meleeMonsterData[0].detectRange)
            {
                // 자살하기
                owner.ChangeState(DemonBomb.State.Suicide);
            }
        }
    }
}