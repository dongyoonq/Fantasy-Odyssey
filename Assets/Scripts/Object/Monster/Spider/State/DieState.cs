using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SpiderState
{
    public class DieState : MonsterBaseState<Spider>
    {
        public DieState(Spider owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Die", true);
            owner.GetComponent<CharacterController>().enabled = false;
            Player.Instance.OnChangeKillQuestUpdate?.Invoke(owner.data);
            owner.DropItemAndUpdateExp();
            owner.StartCoroutine(disaapearRoutine());
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }

        IEnumerator disaapearRoutine()
        {
            yield return new WaitForSeconds(2.5f);
            owner.animator.SetBool("Disappear", true);
            yield return new WaitForSeconds(1f);

            owner.spawnInfo.currMonster--;
            int index = Array.IndexOf(owner.spawnInfo.monters, owner);
            owner.spawnInfo.spawnPoint[index].state = SpawnPoint.State.Empty;
            owner.spawnInfo.monters[index] = null;
            GameManager.Resource.Destroy(owner.gameObject);
        }
    }
}