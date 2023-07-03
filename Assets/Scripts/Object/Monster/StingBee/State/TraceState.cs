using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace StingBeeState
{
    public class TraceState : MonsterBaseState<StingBee>
    {
        bool isWait;

        public TraceState(StingBee owner) : base(owner)
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
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < 1.5f)
                owner.animator.SetBool("Move", false);
            else
                owner.transform.Translate(new Vector3(TargetDir.x, 0, TargetDir.z) * owner.data.moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);

            if (Vector3.Distance(Player.Instance.transform.position, owner.transform.position) < 4f && !isWait)
            {
                owner.ChangeState(StingBee.State.Attack);
            }
        }

        IEnumerator CoolTimer()
        {
            yield return new WaitForSeconds(owner.coolTime);
            isWait = false;
        }
    }
}