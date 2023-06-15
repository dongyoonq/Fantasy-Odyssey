using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    // �� ���⸦ ����� ���� �ִϸ�����
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public string Name { get { return _name; } }
    public float AttackPower { get { return attackPower; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }
    public int ReqLvl { get { return requireLevel; } }
    public string ReqJob { get { return requireJob; } }

    [Header("���� ����")]
    [SerializeField] RuntimeAnimatorController weaponAnimator;
    [SerializeField] string _name;
    [SerializeField] float attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;
    [SerializeField] int requireLevel;
    [SerializeField] string requireJob;
}