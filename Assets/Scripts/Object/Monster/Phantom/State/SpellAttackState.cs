using System.Collections;
using UnityEngine;

namespace PhantomState
{
    public class SpellAttackState : MonsterBaseState<Phantom>
    {
        ParticleSystem particle;

        public SpellAttackState(Phantom owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Spell", true);
            owner.StartCoroutine(SpellAttackRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Spell", false);
            owner.StopCoroutine(SpellAttackRoutine());
            GameManager.Resource.Destroy(particle.gameObject, 0.25f);
            owner.coolTime = 1f;
        }

        public override void Update()
        {

        }

        IEnumerator SpellAttackRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Monster/Phantom/BarrageIce", Player.Instance.transform.position + (Player.Instance.transform.up * 1f), Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
            BarrageIceHitBox hitBox = particle.transform.GetChild(4).GetComponent<BarrageIceHitBox>();
            hitBox.owner = owner;
            yield return new WaitForSeconds(0.5f);
            hitBox.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            owner.ChangeState(Phantom.State.Trace);
        }
    }
}