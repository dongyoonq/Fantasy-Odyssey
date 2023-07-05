using System.Collections;
using UnityEngine;

namespace PhantomState
{
    public class IdleState : MonsterBaseState<Phantom>
    {
        float waitTime;

        public IdleState(Phantom owner) : base(owner)
        {
        }

        public override void Enter()
        {
            waitTime = Random.Range(1f, 3f);
            owner.roamingCoolTime = owner.StartCoroutine(RoamingCoolTime());
        }

        public override void Exit()
        {
            if (owner.roamingCoolTime != null)
                owner.StopCoroutine(owner.roamingCoolTime);
        }

        public override void Update()
        {
            if (Vector3.Distance(owner.transform.position, Player.Instance.transform.position) < owner.data.agressiveMonsterData[0].detectRange)
            {
                owner.ChangeState(Phantom.State.Trace);
            }
        }

        IEnumerator RoamingCoolTime()
        {
            yield return new WaitForSeconds(waitTime);
            owner.ChangeState(Phantom.State.Roaming);
        }
    }
}