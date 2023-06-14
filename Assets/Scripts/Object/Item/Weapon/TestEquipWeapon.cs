using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEquipWeapon : MonoBehaviour
{
    [SerializeField] Equipment weapon;

    public void Equip()
    {
        Player.Instance.AddItemToInventory(weapon);
        Equipment equip = Instantiate(weapon);
        Player.Instance.OnEquip(equip);
    }
}
