using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class DieState : MonsterBaseState<DemonBoss>
    {
        public DieState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            if (owner.summon1.IsValid())
                GameManager.Resource.Destroy(owner.summon1.gameObject);

            if (owner.summon2.IsValid())
                GameManager.Resource.Destroy(owner.summon2.gameObject);

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
            yield return new WaitForSeconds(25f);
            owner.animator.SetBool("Die", false);
            owner.animator.SetBool("Disappear", true);
            yield return new WaitForSeconds(2.5f);
            GameManager.Resource.Destroy(owner.gameObject);
        }
    }
}