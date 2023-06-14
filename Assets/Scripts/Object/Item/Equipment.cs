using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item
{
    public EquipmentType type;

    public enum EquipmentType
    {
        Weapon,
        Armor,
        Bottom,
        Shoes,
        Glove,
        Potion,
        Other
    }

    public abstract void ApplyStatusModifier(Player player);
    public abstract void RemoveStatusModifier(Player player);
}