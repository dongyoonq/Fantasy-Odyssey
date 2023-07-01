using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class SpawnState : MonsterBaseState<DemonBoss>
    {
        public SpawnState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.StartCoroutine(SpawnRoutine());
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }

        IEnumerator SpawnRoutine()
        {
            owner.animator.SetTrigger("Spawn");
            yield return new WaitForSeconds(0.9f);
            owner.ChangeState(DemonBoss.State.Idle);
        }
    }
}