using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipmentUI : PopUpUI
{
    public GameObject equipmentPanel;
    public bool activeEquipment = false;

    public EquipmentSlot[] slots;

    Sprite[] orgSprite = new Sprite[8];

    private void Start()
    {
        Player.Instance.equipUI = this;
        slots = new EquipmentSlot[8];
        slots = equipmentPanel.transform.GetChild(0).GetComponentsInChildren<EquipmentSlot>();

        slots[0].equipType = EquipmentData.EquipType.Head;
        slots[1].equipType = EquipmentData.EquipType.Weapon;
        slots[2].equipType = EquipmentData.EquipType.Armor;
        slots[3].equipType = EquipmentData.EquipType.Shield;
        slots[4].equipType = EquipmentData.EquipType.Pants;
        slots[5].equipType = EquipmentData.EquipType.Gloves;
        slots[6].equipType = EquipmentData.EquipType.Cloak;
        slots[7].equipType = EquipmentData.EquipType.Boots;

        for (int i = 0; i < slots.Length; i++)
            orgSprite[i] = slots[i].transform.GetChild(0).GetComponent<Image>().sprite;

        Player.Instance.OnChangeEquipment.AddListener(UpdateEquipmentUI);
        equipmentPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => { OpenEquipment(); GameManager.Sound.PlaySFX("Click"); });
    }

    public void OpenEquipment()
    {
        GameManager.Sound.PlaySFX("OpenUI");
        activeEquipment = !activeEquipment;
        equipmentPanel.transform.GetChild(0).gameObject.SetActive(activeEquipment);
        equipmentPanel.transform.GetChild(0).localPosition = new Vector2(0, 0);
        equipmentPanel.transform.SetAsLastSibling();
    }

    public void OpenEquipment(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Sound.PlaySFX("OpenUI");
            activeEquipment = !activeEquipment;
            equipmentPanel.transform.GetChild(0).gameObject.SetActive(activeEquipment);
            equipmentPanel.transform.GetChild(0).localPosition = new Vector2(0, 0);
            equipmentPanel.transform.SetAsLastSibling();
        }
    }

    public void UpdateEquipmentUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Player.Instance.wearingEquip.ContainsKey(slots[i].equipType))
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Player.Instance.wearingEquip[slots[i].equipType].Data.sprite;
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                slots[i].data = Player.Instance.wearingEquip[slots[i].equipType].Data as EquipmentData;
            }
            else
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = orgSprite[i];
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
            }
        }
    }
}
