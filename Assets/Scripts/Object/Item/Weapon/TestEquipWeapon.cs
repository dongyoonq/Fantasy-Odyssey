using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEquipWeapon : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    public void Equip()
    {
        Player.Instance.AddItemToInventory(weapon);

        //Equipment equip = Instantiate(weapon);
        //Player.Instance.OnEquip(equip);
    }
}
