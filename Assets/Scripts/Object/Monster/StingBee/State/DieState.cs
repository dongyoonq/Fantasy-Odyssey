using System;
using System.Collections;
using UnityEngine;

namespace StingBeeState
{
    public class DieState : MonsterBaseState<StingBee>
    {
        public DieState(StingBee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            GameManager.Sound.PlaySFX("BeeDie");
            owner.animator.SetBool("Die", true);
            owner.StartCoroutine(DissapearRoutine());
            owner.GetComponent<CharacterController>().enabled = false;
            Player.Instance.OnChangeKillQuestUpdate?.Invoke(owner.data);
            owner.DropItemAndUpdateExp();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }

        IEnumerator DissapearRoutine()
        {
            yield return new WaitForSeconds(2.5f);
            owner.animator.SetBool("Die", false);
            owner.animator.SetBool("Disappear", true);
            yield return new WaitForSeconds(1f);
            GameManager.Resource.Destroy(owner.gameObject);

            owner.spawnInfo.currMonster--;
            int index = Array.IndexOf(owner.spawnInfo.monters, owner);
            owner.spawnInfo.spawnPoint[index].state = SpawnPoint.State.Empty;
            owner.spawnInfo.monters[index] = null;
            GameManager.Resource.Destroy(owner.gameObject);
        }
    }
}