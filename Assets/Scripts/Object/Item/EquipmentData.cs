using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "data/Equipment")]
public class EquipmentData : ScriptableObject
{
    public enum EquipType
    {
        Weapon,
        Armor,
        Shoes,
        Gloves
    }

    public EquipType equipType;

    public int Hp;
    public int Mp;

    public int ad;
    public int ap;
}
