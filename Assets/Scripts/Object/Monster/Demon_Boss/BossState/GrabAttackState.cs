using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demon_Boss
{
    public class GrabAttackState : MonsterBaseState<DemonBoss>
    {
        Vector3 prevPlayerPos;
        bool isGrabbed;

        public GrabAttackState(DemonBoss owner) : base(owner)
        {
        }

        public override void Enter()
        {
            isGrabbed = false;
            owner.grabAttackRoutine = owner.StartCoroutine(AnimationRoutine());
        }

        public override void Exit()
        {
            if (isGrabbed)
            {
                Player.Instance.GetComponent<PlayerController>().enabled = true;
                Player.Instance.transform.parent = GameManager.Instance.transform;
                Player.Instance.transform.position = prevPlayerPos;
                Player.Instance.transform.rotation = Quaternion.identity;
                Player.Instance.GetComponent<CharacterController>().enabled = true;
                Player.Instance.GetComponent<PlayerInput>().enabled = true;
            }

            owner.animator.SetBool($"Grab", false);
            owner.StopCoroutine(owner.grabAttackRoutine);
            if (!owner.pharse2)
                owner.coolTime = 1.5f;
            else
                owner.coolTime = 1f;
            owner.patternChangeTimer = 0f;
        }

        public override void Update()
        {
            if (Player.Instance.CurrentHP <= 0)
            {
                Exit();
                owner.ChangeState(DemonBoss.State.Move);
            }

            Vector3 targetDir = (Player.Instance.transform.position - owner.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, Quaternion.Euler(0, targetRot.eulerAngles.y, 0), owner.data.rotSpeed * Time.deltaTime);
        }

        IEnumerator AnimationRoutine()
        {
            owner.animator.SetFloat("MoveSpeed", 10);

            Vector3 start = owner.transform.position;
            Vector3 end = Player.Instance.transform.position;

            float totalTime = Vector3.Distance(start, end) / 10;
            float rate = 0f;

            RaycastHit hit;
            Physics.Raycast(owner.transform.position, (end - start).normalized, out hit, LayerMask.GetMask("Player"));

            end += hit.normal * 3f;

            while (rate < 1f)
            {
                rate += Time.deltaTime / totalTime;

                Vector3 targetPosition = Vector3.Lerp(start, new Vector3(end.x, start.y, end.z), rate);

                // Calculate the movement direction and speed
                Vector3 moveDirection = (targetPosition - owner.transform.position).normalized;

                // Move the character using the Character Controller
                owner.controller.Move(moveDirection * owner.data.moveSpeed * Time.deltaTime);

                yield return null;
            }

            owner.animator.SetBool($"Grab", true);
            GameManager.Sound.PlaySFX("Grab");

            // animation Timing
            yield return new WaitForSeconds(0.5f);
            AttackCircleJudgement(0, 5, 180, 360);
            yield return new WaitForSeconds(3f);

            owner.animator.SetFloat("MoveSpeed", 0);
            owner.ChangeState(DemonBoss.State.Move);
        }

        void AttackCircleJudgement(int damage, float range, float forwardAngle, float upAngle)
        {
            Collider[] colliders = Physics.OverlapSphere(owner.transform.position, range, LayerMask.GetMask("Player"));
            foreach (Collider collider in colliders)
            {
                Vector3 dirTarget = (collider.transform.position - owner.transform.position).normalized;

                if (Vector3.Dot(owner.transform.up, dirTarget) >= Mathf.Cos(upAngle * 0.5f * Mathf.Deg2Rad) &&
                    Vector2.Dot(owner.transform.forward, dirTarget) >= Mathf.Cos(forwardAngle * 0.5f * Mathf.Deg2Rad))
                {
                    owner.StartCoroutine(DamageRoutine());
                    isGrabbed = true;
                    prevPlayerPos = Player.Instance.transform.position;
                    Player.Instance.transform.parent = owner.lefthand.transform;
                    Player.Instance.transform.localPosition = new Vector3(0.209f, -0.162f, -0.188f);
                    Player.Instance.transform.localRotation = Quaternion.Euler(56.507f, -301.758f, -277.772f);
                    Player.Instance.YSpeed = 0;
                    Player.Instance.GetComponent<PlayerInput>().enabled = false;
                    Player.Instance.GetComponent<CharacterController>().enabled = false;
                    Player.Instance.GetComponent<PlayerController>().enabled = false;
                }

            }
        }

        IEnumerator DamageRoutine()
        {
            int count = 0;

            while (count < 3)
            {
                Player.Instance.Hit(250);
                count++;
                yield return new WaitForSeconds(1.3f);
            }
        }
    }
}