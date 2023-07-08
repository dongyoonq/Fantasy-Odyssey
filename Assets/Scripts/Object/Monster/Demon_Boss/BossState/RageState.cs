using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Demon_Boss
{
    public class RageState : MonsterBaseState<DemonBoss>
    {
        public RageState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            GameManager.Sound.PlaySFX("Rage");
            owner.animator.SetBool("Rage", true);
            owner.rageRoutine = owner.StartCoroutine(RageRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Rage", false);
            owner.StopCoroutine(owner.rageRoutine);
        }

        public override void Update()
        {

        }

        IEnumerator RageRoutine()
        {
            Color startColor = owner.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color;

            float elapsedTime = 0f;

            while (elapsedTime < 3f)
            {
                Color currentColor = Color.Lerp(startColor, new Color(0.4509804f, 0, 1, 1), elapsedTime / 3f);

                owner.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color = currentColor;

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            owner.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color = new Color(0.4509804f, 0, 1, 1);
            owner.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", new Color(1, 0, 0, 1) * 10f);
            string inKeyword = "_EMISSION";
            LocalKeyword keyword = new LocalKeyword(owner.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.shader, inKeyword);
            owner.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.EnableKeyword(keyword);
            owner.patternChangeInterval = 10f;
            owner.ChangeState(DemonBoss.State.Idle);
        }
    }
}