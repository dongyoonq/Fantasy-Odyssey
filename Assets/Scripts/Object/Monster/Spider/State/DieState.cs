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

            owner.spawnInfo.currSpider--;
            int index = Array.IndexOf(owner.spawnInfo.spiders, owner);
            owner.spawnInfo.spawnPoint[index].state = SpiderSpawnPoint.State.Empty;
            owner.spawnInfo.spiders[index] = null;
            GameManager.Resouce.Destroy(owner.gameObject);
        }
    }
}