using System.Collections;
using UnityEngine;

namespace PhantomState
{
    public class TraceState : MonsterBaseState<Phantom>
    {
        bool isWait;

        public TraceState(Phantom owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isWait = true;
            owner.animator.SetBool("Move", true);
            owner.StartCoroutine(CoolTimer());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Move", false);
        }

        public override void Update()
        {
            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < 1.5f)
                owner.animator.SetBool("Move", false);
            else
            {
                if (owner.controller.enabled)
                    owner.controller.Move(targetDir * owner.data.moveSpeed * Time.deltaTime);
            }

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

            if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.data.meleeMonsterData[0].detectRange && !isWait)
            {
                owner.ChangeState(Phantom.State.Attack);
            }
            else if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < owner.data.rangeMonsterData[0].detectRange && !isWait)
            {
                owner.ChangeState(Phantom.State.SpellAttack);
            }
            else if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) > owner.data.agressiveMonsterData[0].detectRange)
            {
                owner.ChangeState(Phantom.State.Return);
            }
        }

        IEnumerator CoolTimer()
        {
            yield return new WaitForSeconds(owner.coolTime);
            isWait = false;
        }
    }
}