using System.Collections;
using UnityEngine;

namespace Demon_Bomb
{
    public class SpawnState : MonsterBaseState<DemonBomb>
    {
        public SpawnState(DemonBomb owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Spawn", true);
            owner.StartCoroutine(SpawnRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Spawn", false);
        }

        public override void Update()
        {

        }

        IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(1.8f);
            owner.ChangeState(DemonBomb.State.Move);
        }
    }
}