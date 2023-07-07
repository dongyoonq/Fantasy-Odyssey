using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class MoveState : MonsterBaseState<DemonBoss>
    {
        bool isWait;
        float lastSpeed;

        public MoveState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isWait = true;
            owner.StartCoroutine(CoolTimeRoutine());
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.meleeMonsterData[0].detectRange && !isWait)
            {
                owner.ChangeState(DemonBoss.State.Claw);
            }
            else if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.rangeMonsterData[0].detectRange && !isWait && owner.pharse2)
            {
                if (owner.currState == DemonBoss.State.Idle || owner.currState == DemonBoss.State.Move)
                    owner.ChangeState(DemonBoss.State.Throw);
            }
            else if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.agressiveMonsterData[0].detectRange)
            {
                if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < 3.2f)
                {
                    owner.animator.SetFloat("MoveSpeed", 0);
                    return;
                }

                lastSpeed = owner.data.moveSpeed;
                if (owner.controller.enabled)
                    owner.controller.Move(targetDir * lastSpeed * Time.deltaTime);
            }
            else if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.agressiveMonsterData[1].detectRange)
            {
                if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < 3.2f)
                {
                    owner.animator.SetFloat("MoveSpeed", 0);
                    return;
                }

                lastSpeed = owner.data.moveSpeed + 3f;
                if (owner.controller.enabled)
                    owner.controller.Move(targetDir * lastSpeed * Time.deltaTime);
            }

            owner.animator.SetFloat("MoveSpeed", lastSpeed);
        }

        IEnumerator CoolTimeRoutine()
        {
            yield return new WaitForSeconds(owner.coolTime);
            isWait = false;
        }
    }
}