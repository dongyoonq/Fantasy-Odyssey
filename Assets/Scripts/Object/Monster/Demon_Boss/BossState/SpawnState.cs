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
            owner.animator.SetBool("Spawn", true);
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
            yield return new WaitForSeconds(1.6f);
            owner.animator.SetBool("Spawn", false);
            owner.ChangeState(DemonBoss.State.Idle);
        }
    }
}