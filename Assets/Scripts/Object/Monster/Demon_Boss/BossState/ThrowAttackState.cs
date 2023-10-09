using System.Collections;
using System.Net;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Demon_Boss
{
    public class ThrowAttackState : MonsterBaseState<DemonBoss>
    {
        GameObject rock;

        public ThrowAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.rockElapseTime = 0;
            owner.animator.SetBool("Throw", true);
            owner.rockAttackRoutine = owner.StartCoroutine(ThrowRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Throw", false);

            if (owner.rockAttackRoutine != null)
            {
                owner.StopCoroutine(owner.rockAttackRoutine);
                if (rock.IsValid())
                    GameManager.Resource.Destroy(rock.gameObject);
            }

            owner.coolTime = 0.3f;
        }

        public override void Update()
        {
            Vector3 TargetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(TargetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }

        IEnumerator ThrowRoutine()
        {
            yield return new WaitForSeconds(0.65f);
            rock = GameManager.Resource.Instantiate<GameObject>("Prefabs/Monster/DemonBoss/Rock", owner.righthand.transform.position + (owner.transform.up * 0.5f), Quaternion.identity);
            rock.GetComponent<BossRock>().owner = owner;
            yield return new WaitForSeconds(0.6f);
            GameManager.Sound.PlaySFX("Throw");

            Vector3 start = rock.transform.position;
            Vector3 end = Player.Instance.transform.position;
            Vector3 center = ((start + end) / 2) + (Vector3.up * 10f);

            float time = 0f;
            float duration = 1f;
            owner.rockElapseTime = duration;

            while (time < duration)
            {
                Vector3 temp1 = Vector3.Lerp(start, center, time);
                Vector3 temp2 = Vector3.Lerp(center, end, time);
                rock.transform.position = Vector3.Lerp(temp1, temp2, time);

                time += Time.deltaTime / duration;

                yield return null;
            }

            owner.ChangeState(DemonBoss.State.Move);

            if (rock.IsValid())
                rock.transform.position = end;

            yield return null;

            if (rock.IsValid())
                GameManager.Resource.Destroy(rock.gameObject, 0.5f);
        }
    }
}