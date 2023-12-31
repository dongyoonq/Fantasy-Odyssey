using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class IdleState : MonsterBaseState<Spider>
    {
        public IdleState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.data.agressiveMonsterData[0].detectRange)
            {
                owner.ChangeState(Spider.State.Trace);
            }
        }
    }
}