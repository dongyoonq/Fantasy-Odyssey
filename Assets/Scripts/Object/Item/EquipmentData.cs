using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentData : ItemData
{
    public int ReqLvl { get { return requireLevel; } set { requireLevel = value; } }
    public string ReqJob { get { return requireJob; } set { requireJob = value; } }

    public enum EquipType
    {
        Weapon,
        Shield,
        Head,
        Armor,
        Pants,
        Boots,
        Gloves,
        Cloak,
    }

    [SerializeField] int requireLevel;
    [SerializeField] string requireJob;
    public EquipType equipType;
}
