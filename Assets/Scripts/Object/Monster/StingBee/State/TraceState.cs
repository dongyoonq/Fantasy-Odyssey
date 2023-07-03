using System.Collections;
using UnityEngine;

namespace StingBeeState
{
    public class TraceState : MonsterBaseState<StingBee>
    {
        public TraceState(StingBee owner) : base(owner)
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

            owner.transform.Translate(new Vector3(TargetDir.x, 0, TargetDir.z) * owner.data.moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }
    }
}