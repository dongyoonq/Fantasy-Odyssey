using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Equipment
{
    // 무기의 현재 콤보 카운트
    public int ComboCount { get; set; }
    // 이 무기를 쥘 때의 로컬 좌표 정보
    public WeaponData WeaponData { get { return weaponData; } }

    [SerializeField] protected WeaponData weaponData;

    public abstract void Attack();         // 기본 공격
    public abstract void DashAttack();     // 대시 공격
    public abstract void ChargingAttack(); // 차지 공격
    public abstract void Skill();          // 스킬
    public abstract void UltimateSkill();  // 궁극기
}