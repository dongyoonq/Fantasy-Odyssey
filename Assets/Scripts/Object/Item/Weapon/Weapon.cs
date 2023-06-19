using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Equipment
{
    // ������ ���� �޺� ī��Ʈ
    public int ComboCount { get; set; }
    public int TotalDamage {  get; set; }
    // �� ���⸦ �� ���� ���� ��ǥ ����
    public WeaponData WeaponData { get { return weaponData; } }

    [SerializeField] protected WeaponData weaponData;

    public abstract void LeftAttack();         // �⺻ ����
    public abstract void DashAttack();     // ��� ����
    public abstract void ChargingAttack(); // ���� ����
    public abstract void Skill();          // ��ų
    public abstract void UltimateSkill();  // �ñر�

    protected void AttackJudgement(int damage, float range, float forwardAngle, float upAngle)
    {
        Collider[] colliders = Physics.OverlapSphere(Player.Instance.transform.position, range, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            Vector3 dirTarget = (collider.transform.position - Player.Instance.transform.position).normalized;
            Vector2 dir2DTarget = new Vector2(dirTarget.x, dirTarget.z);

            if (Vector3.Dot(Player.Instance.transform.up, dirTarget) >= Mathf.Cos(upAngle * 0.5f * Mathf.Deg2Rad))
            {
                if (Vector2.Dot(Player.Instance.transform.forward, dir2DTarget) >= Mathf.Cos(forwardAngle * 0.5f * Mathf.Deg2Rad))
                    collider.GetComponent<IHitable>().Hit(damage);
            }
        }
    }

    protected Vector3 AngleToDir(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }
}