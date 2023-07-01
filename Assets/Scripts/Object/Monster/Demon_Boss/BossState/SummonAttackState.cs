using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class SummonAttackState : MonsterBaseState<DemonBoss>
    {
        DemonBomb summon1;
        DemonBomb summon2;

        public SummonAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.summonAttackRoutine = owner.StartCoroutine(SummonRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.summonAttackRoutine);
            owner.coolTime = 1;
            owner.patternChangeTimer = 0f;
        }

        public override void Update()
        {
        }

        IEnumerator SummonRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            owner.animator.SetBool("Summon1", true);
            summon1 = GameManager.Resource.Instantiate<DemonBomb>("Prefabs/Monster/DemonBoss/demon_bomb", owner.summonPos1.position, owner.transform.rotation);
            yield return new WaitForSeconds(0.5f);
            owner.animator.SetBool("Summon1", false);
            owner.animator.SetBool("Summon2", true);
            summon2 = GameManager.Resource.Instantiate<DemonBomb>("Prefabs/Monster/DemonBoss/demon_bomb", owner.summonPos2.position, owner.transform.rotation);
            yield return new WaitForSeconds(0.5f);
            owner.animator.SetBool("Summon2", false);
            yield return new WaitForSeconds(0.5f);
            owner.ChangeState(DemonBoss.State.Move);
        }
    }
}