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
            owner.coolTime = 1f;

            if (food.IsValid())
                GameManager.Resource.Destroy(food.gameObject);
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
            GameManager.Ui.SetFloating(owner.gameObject, +400, new Color(0,1,0,1));
            if (owner.currHp >= owner.data.maxHp)
                owner.currHp = owner.data.maxHp;
            GameManager.Resource.Destroy(food.gameObject);
            yield return new WaitForSeconds(1.5f);

            owner.ChangeState(DemonBoss.State.Move);
        }
    }
}