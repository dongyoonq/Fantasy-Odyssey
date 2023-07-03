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
            owner.animator.SetBool("Die", true);
            owner.StartCoroutine(DissapearRoutine());
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
        }
    }
}