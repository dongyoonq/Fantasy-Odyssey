using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Equipment
{
    // 무기의 현재 콤보 카운트
    public int ComboCount { get; set; }
    public int TotalDamage {  get; set; }
    // 이 무기를 쥘 때의 로컬 좌표 정보
    public WeaponData WeaponData { get { return weaponData; } }

    [SerializeField] protected WeaponData weaponData;

    public abstract void LeftAttack();         // 기본 공격
    public abstract void DashAttack();     // 대시 공격
    public abstract void ChargingAttack(); // 차지 공격
    public abstract void Skill();          // 스킬
    public abstract void UltimateSkill();  // 궁극기

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