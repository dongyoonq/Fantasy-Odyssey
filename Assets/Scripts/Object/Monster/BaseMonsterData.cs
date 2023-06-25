using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseMonster Data", menuName = "Scriptable Object/BaseMonster Data", order = 1)]
public class BaseMonsterData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("몬스터 정보")]
    [SerializeField] List<AgressiveMonsterData> _agreesiveMonsterData = new List<AgressiveMonsterData>();
    [SerializeField] List<MeleeMonsterData> _meleeMonsterData = new List<MeleeMonsterData>();
    [SerializeField] List<RangeMonsterData> _rangeMonsterData = new List<RangeMonsterData>();
    [SerializeField] List<ItemData> _dropTable = new List<ItemData>();
    [SerializeField] int _maxHp;
    [SerializeField] int _moveSpeed;
    [SerializeField] int _rotSpeed;

    [NonSerialized] public List<AgressiveMonsterData> AgressiveMonsterData;
    [NonSerialized] public List<MeleeMonsterData> MeleeMonsterData;
    [NonSerialized] public List<RangeMonsterData> RangeMonsterData;
    [NonSerialized] public List<ItemData> DropTable;
    [NonSerialized] public int MaxHp;
    [NonSerialized] public float MoveSpeed;
    [NonSerialized] public float RotSpeed;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        AgressiveMonsterData = _agreesiveMonsterData;
        MeleeMonsterData = _meleeMonsterData;
        RangeMonsterData = _rangeMonsterData;
        DropTable = _dropTable;
        MaxHp = _maxHp;
        MoveSpeed = _moveSpeed;
        RotSpeed = _rotSpeed;
    }
}