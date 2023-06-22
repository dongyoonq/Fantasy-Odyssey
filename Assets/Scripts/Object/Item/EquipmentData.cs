using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Data", menuName = "Scriptable Object/Equipment Data", order = 100000000)]
public abstract class EquipmentData : ItemData
{
    public int ReqLvl { get { return requireLevel; } }
    public string ReqJob { get { return requireJob; } }

    public enum EquipType
    {
        Weapon,
        Armor,
        Bottom,
        Shoes,
        Glove,
        Potion,
        Other
    }

    [SerializeField] int requireLevel;
    [SerializeField] string requireJob;
    public EquipType equipType;
}
