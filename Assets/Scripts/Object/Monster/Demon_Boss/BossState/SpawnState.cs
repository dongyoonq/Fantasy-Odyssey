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
            if (owner.spawnParticle.IsValid())
                GameManager.Resource.Destroy(owner.spawnParticle.gameObject, 5f);
            owner.animator.SetBool("Spawn", false);
        }

        public override void Update()
        {

        }

        IEnumerator SpawnRoutine()
        {
            owner.spawnParticle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Portal/BossSpawnPortal", owner.transform.position + (owner.transform.up * 0.2f), Quaternion.Euler(90f, 0f, 0f));
            yield return new WaitForSeconds(1.6f);
            owner.ChangeState(DemonBoss.State.Idle);
        }
    }
}