
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public Weapon(EquipmentData data) : base(data) 
    {
    }

    // 무기의 현재 콤보 카운트
    public int ComboCount { get; set; }
    public int TotalDamage {  get; set; }
    // 이 무기를 쥘 때의 로컬 좌표 정보

    public WeaponData weaponData;

    public virtual void LeftAttack() { }     // 기본 공격
    public virtual void RightAttack() { }
    public virtual void DashAttack() { }      // 대시 공격
    public virtual void ChargingAttack() { }  // 차지 공격
    public virtual void Skill() { }           // 스킬
    public virtual void UltimateSkill() { }   // 궁극기

    protected void AttackCircleJudgement(int damage, float range, float forwardAngle, float upAngle)
    {
        Collider[] colliders = Physics.OverlapSphere(Player.Instance.transform.position, range, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            Vector3 dirTarget = (collider.transform.position - Player.Instance.transform.position).normalized;

            if (Vector3.Dot(Player.Instance.transform.up, dirTarget) >= Mathf.Cos(upAngle * 0.5f * Mathf.Deg2Rad) &&
                Vector2.Dot(Player.Instance.transform.forward, dirTarget) >= Mathf.Cos(forwardAngle * 0.5f * Mathf.Deg2Rad))
                collider.GetComponent<IHitable>().Hit(damage);
        }
    }

    protected void AttackBoxJudgement(int damage, Vector3 center, Vector3 size, Quaternion rotation)
    {
        Collider[] colliders = Physics.OverlapBox(center,
            size, rotation, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
            collider.GetComponent<IHitable>().Hit(damage);
    }

    protected Vector3 AngleToDir(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }


    private void OnDrawGizmosSelected()
    {
        if (!weaponData.debug)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.2f);

        // LeftAttackRange
        /*
         * leftattackfinish angle is 360
        Vector3 rightDir = AngleToDir(Player.Instance.transform.eulerAngles.y + 200 * 0.5f);    
        Vector3 leftDir = AngleToDir(Player.Instance.transform.eulerAngles.y - 200 * 0.5f);
        Debug.DrawRay(Player.Instance.transform.position, rightDir * 2.2f, Color.red);
        Debug.DrawRay(Player.Instance.transform.position, leftDir * 2.2f, Color.red);
        */

        // RightAttackRange
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireCube(transform.position + transform.forward * 1f, new Vector3(0.4f,0.4f,1.8f));
    }

    public override void ApplyStatusModifier()
    {

    }

    public override void RemoveStatusModifier()
    {

    }

    public override void Use()
    {

    }
}