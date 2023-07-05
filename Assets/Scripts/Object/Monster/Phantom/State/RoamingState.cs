using System.Collections;
using UnityEngine;

namespace PhantomState
{
    public class RoamingState : MonsterBaseState<Phantom>
    {
        int randomPoint;

        public RoamingState(Phantom owner) : base(owner)
        {
        }

        public override void Enter()
        {
            randomPoint = Random.Range(0, owner.roamingPoint.Length);

            if (randomPoint == owner.prevPoint)
            {
                while (randomPoint != owner.prevPoint)
                {
                    randomPoint = Random.Range(0, owner.roamingPoint.Length);
                }
            }

            owner.animator.SetBool("Move", true);
        }

        public override void Exit()
        {
            owner.animator.SetBool("Move", false);
            owner.prevPoint = randomPoint;
        }

        public override void Update()
        {
            Vector3 targetDir = (owner.roamingPoint[randomPoint].transform.position - owner.transform.position).normalized;
            if (owner.controller.enabled)
                owner.controller.Move(targetDir * owner.data.moveSpeed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.agressiveMonsterData[0].detectRange)
            {
                owner.ChangeState(Phantom.State.Trace);
            }
            else if (Vector3.Distance(owner.transform.position, owner.roamingPoint[randomPoint].transform.position) < 0.3f)
            {
                owner.ChangeState(Phantom.State.Idle);
            }
        }
    }
}