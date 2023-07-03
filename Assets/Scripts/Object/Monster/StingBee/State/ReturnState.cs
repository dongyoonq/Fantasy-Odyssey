using System;
using System.Collections;
using UnityEngine;

namespace StingBeeState
{
    public class ReturnState : MonsterBaseState<StingBee>
    {
        public ReturnState(StingBee owner) : base(owner)
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
            if (Vector3.Distance(owner.transform.position, owner.spawnPos) < 0.5f)
            {
                if (owner.IsValid())
                    if (Array.IndexOf(owner.spawnInfo.monters, owner) >= 0)
                        owner.transform.rotation = owner.spawnInfo.spawnPoint[Array.IndexOf(owner.spawnInfo.monters, owner)].transform.rotation;
                owner.ChangeState(StingBee.State.Idle);
            }

            Vector3 TargetDir = (owner.spawnPos - owner.transform.position).normalized;

            owner.transform.Translate(new Vector3(TargetDir.x, 0, TargetDir.z) * owner.data.moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }
    }
}