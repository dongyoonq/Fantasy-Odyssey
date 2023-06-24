using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEquipWeapon : MonoBehaviour
{
    //[SerializeField] WeaponData WeaponData;
    [SerializeField] ItemData itemData;

    public void Equip()
    {
        Player.Instance.AddItemToInventory(itemData);
    }
}
