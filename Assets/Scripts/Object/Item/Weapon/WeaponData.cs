using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    public bool debug;

    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    // 이 무기를 사용할 때의 애니메이터
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public string Name { get { return _name; } }
    public int AttackPower { get { return attackPower; } }
    public float AttackSpeed { get { return attackSpeed; } }
    //public float AttackRange { get { return attackRange; } }
    public float MaxCombo { get { return maxCombo; } }
    public int ReqLvl { get { return requireLevel; } }
    public string ReqJob { get { return requireJob; } }
    public List<ParticleSystem> Effects { get { return effects; } }

    [Header("무기 정보")]
    [SerializeField] RuntimeAnimatorController weaponAnimator;
    [SerializeField] string _name;
    [SerializeField] int attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;
    [SerializeField] int maxCombo;
    [SerializeField] int requireLevel;
    [SerializeField] string requireJob;

    [SerializeField] List<ParticleSystem> effects = new List<ParticleSystem>();
}