using System.Collections;
using System.Net;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Demon_Boss
{
    public class ThrowAttackState : MonsterBaseState<DemonBoss>
    {
        GameObject rock;
        float time = 1f;

        public ThrowAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.animator.SetBool("Throw", true);
            owner.rockAttackRoutine = owner.StartCoroutine(ThrowRoutine());
        }

        public override void Exit()
        {
            owner.animator.SetBool("Throw", false);
            owner.coolTime = 1f;
        }

        public override void Update()
        {

        }

        IEnumerator ThrowRoutine()
        {
            yield return new WaitForSeconds(0.65f);
            rock = GameManager.Resource.Instantiate<GameObject>("Prefabs/Monster/DemonBoss/Rock", owner.righthand.transform.position + (owner.transform.up * 0.5f), Quaternion.identity);
            rock.GetComponent<BossRock>().owner = owner;
            yield return new WaitForSeconds(0.7f);

            owner.ChangeState(DemonBoss.State.Move);

            Vector3 start = rock.transform.position;
            Vector3 end = Player.Instance.transform.position;

            float xSpeed = (end.x - start.x) / time;
            float zSpeed = (end.z - start.z) / time;
            float ySpeed = -1 * (0.5f * Physics.gravity.y * time * time + start.y) / time;

            owner.rockElapseTime = time;

            float curTime = 0;

            while (curTime < time)
            {
                curTime += Time.deltaTime;

                if (rock.IsValid())
                    rock.transform.position += new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime;
                ySpeed += Physics.gravity.y * Time.deltaTime;

                yield return null;
            }

            if (rock.IsValid())
                rock.transform.position = end;

            yield return null;

            if (rock.IsValid())
                GameManager.Resource.Destroy(rock, 1.4f);
        }
    }
}