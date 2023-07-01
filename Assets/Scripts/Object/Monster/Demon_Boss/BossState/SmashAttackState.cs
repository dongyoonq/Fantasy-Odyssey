using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Demon_Boss
{
    public class SmashAttackState : MonsterBaseState<DemonBoss>
    {
        ParticleSystem particle;

        public SmashAttackState(DemonBoss owner) : base(owner)
        {

        }
        public override void Enter()
        {
            owner.smashRoutine = owner.StartCoroutine(animationRoutine());
        }

        public override void Exit()
        {
            owner.StopCoroutine(owner.smashRoutine);
            owner.animator.SetBool($"Smash", false);
            owner.coolTime = 1f;
            owner.patternChangeTimer = 0f;
        }

        public override void Update()
        {
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }

        IEnumerator animationRoutine()
        {
            owner.animator.SetFloat("MoveSpeed", 9);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 2f;

            float totalTime = Vector3.Distance(start, end) / 9;
            float rate = 0f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;
                owner.transform.position = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);
                yield return null;
            }

            owner.animator.SetBool($"Smash", true);

            // animation Timing
            yield return new WaitForSeconds(1f);
            GameObject hitBox = GameManager.Resource.Instantiate<GameObject>("Prefabs/Monster/DemonBoss/RockHitBox", owner.transform.position + (owner.transform.forward * 1f), owner.transform.rotation);
            hitBox.GetComponent<RockHitBox>().owner = owner;
            particle = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/Monster/DemonBoss/RockParticle", owner.transform.position + (owner.transform.forward * 1f), owner.transform.rotation, hitBox.transform);
            yield return new WaitForSeconds(0.5f);
            owner.animator.SetFloat("MoveSpeed", 0);
            if (hitBox.IsValid())
                GameManager.Resource.Destroy(hitBox, 3.3f);
            owner.ChangeState(DemonBoss.State.Move);
        }
    }
}