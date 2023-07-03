using System;
using System.Collections;
using UnityEngine;

namespace StingBeeState
{
    public class IdleState : MonsterBaseState<StingBee>
    {
        [NonSerialized] public bool isFindTarget;

        public IdleState(StingBee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isFindTarget = false;
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (!isFindTarget)
                FindTarget();
        }

        void FindTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position, owner.data.agressiveMonsterData[0].detectRange, owner.targetMask);
            foreach (Collider collider in colliders)
            {
                Vector3 dirTarget = (collider.transform.position - owner.transform.position).normalized;
                if (Vector3.Dot(owner.transform.forward, dirTarget) < Mathf.Cos(owner.data.agressiveMonsterData[0].detectAngle * 0.5f * Mathf.Deg2Rad))
                    continue;

                float distanceToTarget = Vector3.Distance(owner.transform.position, collider.transform.position);
                if (Physics.Raycast(owner.transform.position, dirTarget, distanceToTarget, owner.obstacleMask))
                    continue;

                isFindTarget = true;
                Debug.Log("타겟 감지");
                owner.ChangeState(StingBee.State.Trace);
            }
        }
    }
}