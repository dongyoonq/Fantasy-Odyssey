using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Equipment
{
    // ������ ���� �޺� ī��Ʈ
    public int ComboCount { get; set; }
    // �� ���⸦ �� ���� ���� ��ǥ ����
    public WeaponData WeaponData { get { return weaponData; } }

    [SerializeField] protected WeaponData weaponData;

    public abstract void Attack();         // �⺻ ����
    public abstract void DashAttack();     // ��� ����
    public abstract void ChargingAttack(); // ���� ����
    public abstract void Skill();          // ��ų
    public abstract void UltimateSkill();  // �ñر�
}