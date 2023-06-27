using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentData : ItemData
{
    public int ReqLvl { get { return requireLevel; } }
    public string ReqJob { get { return requireJob; } }

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
