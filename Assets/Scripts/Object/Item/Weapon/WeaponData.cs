using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = 100000000)]
public class WeaponData : EquipmentData
{
    public bool debug;

    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    // 이 무기를 사용할 때의 애니메이터
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public int AttackPower { get { return attackPower; } set { attackPower = value; } }
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    //public float AttackRange { get { return attackRange; } }
    public int MaxCombo { get { return maxCombo; } set { maxCombo = value; } }
    public float CoolTimeSkill { get { return coolTimeSkill; } set { coolTimeSkill = value; } }
    public List<ParticleSystem> Effects { get { return effects; } }

    [Header("무기 정보")]
    [SerializeField] RuntimeAnimatorController weaponAnimator;
    [SerializeField] int attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] int maxCombo;
    [SerializeField] float coolTimeSkill;

    [SerializeField] List<ParticleSystem> effects = new List<ParticleSystem>();
}