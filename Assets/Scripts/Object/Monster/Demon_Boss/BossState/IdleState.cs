using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class IdleState : MonsterBaseState<DemonBoss>
    {
        public IdleState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetFloat("MoveSpeed", 0);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.agressiveMonsterData[1].detectRange)
            {
                owner.ChangeState(DemonBoss.State.Move);
            }
        }
    }
}