using System.Collections;
using UnityEngine;

namespace Demon_Boss
{
    public class HealState : MonsterBaseState<DemonBoss>
    {
        GameObject food;

        public HealState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Heal", true);
            owner.healRoutine = owner.StartCoroutine(HealRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Heal", false);
            owner.StopCoroutine(owner.healRoutine);
            owner.coolTime = 0.8f;
        }

        public override void Update()
        {

        }

        IEnumerator HealRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            food = GameManager.Resource.Instantiate<GameObject>("Prefabs/Monster/DemonBoss/Chicken", owner.lefthand.transform.position, Quaternion.identity, owner.lefthand.transform);
            yield return new WaitForSeconds(1.1f);
            owner.currHp += 400;
            if (owner.currHp >= owner.data.maxHp)
                owner.currHp = owner.data.maxHp;
            GameManager.Resource.Destroy(food);
            yield return new WaitForSeconds(1.5f);

            owner.ChangeState(DemonBoss.State.Move);
        }
    }
}