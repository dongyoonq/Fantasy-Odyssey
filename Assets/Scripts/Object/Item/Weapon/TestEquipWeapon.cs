using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEquipWeapon : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    public void Equip()
    {
        //weapon.Data = weapon.weaponData;
        Player.Instance.AddItemToInventory(weapon);
        //Equipment equip = Instantiate(weapon);
        //Player.Instance.OnEquip(equip);
    }
}
