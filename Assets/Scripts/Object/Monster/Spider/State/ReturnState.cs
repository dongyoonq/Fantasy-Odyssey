using System;
using System.Collections;
using UnityEngine;

namespace SpiderState
{
    public class ReturnState : MonsterBaseState<Spider>
    {
        public ReturnState(Spider owner) : base(owner)
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
                owner.ChangeState(Spider.State.Idle);
            }

            Vector3 targetDir = (owner.spawnPos - owner.transform.position).normalized;

            if (owner.controller.enabled)
                owner.controller.Move(targetDir * owner.data.moveSpeed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }
    }
}