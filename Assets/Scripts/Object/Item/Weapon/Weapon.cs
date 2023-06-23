
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public Weapon(EquipmentData data) : base(data) 
    {
    }

    // ������ ���� �޺� ī��Ʈ
    public int ComboCount { get; set; }
    public int TotalDamage {  get; set; }
    // �� ���⸦ �� ���� ���� ��ǥ ����

    public WeaponData weaponData;

    public virtual void LeftAttack() { }     // �⺻ ����
    public virtual void RightAttack() { }
    public virtual void DashAttack() { }      // ��� ����
    public virtual void ChargingAttack() { }  // ���� ����
    public virtual void Skill() { }           // ��ų
    public virtual void UltimateSkill() { }   // �ñر�

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