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
    public GameObject EquipmentPanel;
    public bool activeEquipment = false;

    public EquipmentSlot[] slots;

    private void Start()
    {
        Player.Instance.equipUI = this;
        slots = new EquipmentSlot[8];
        slots = EquipmentPanel.transform.GetChild(0).GetComponentsInChildren<EquipmentSlot>();
    }

    public void OpenEquipment()
    {
        activeEquipment = !activeEquipment;
        GameManager.Ui.activePopupUI = activeEquipment;
        EquipmentPanel.transform.GetChild(0).gameObject.SetActive(activeEquipment);
    }

    public void OpenEquipment(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            activeEquipment = !activeEquipment;
            GameManager.Ui.activePopupUI = activeEquipment;
            EquipmentPanel.transform.GetChild(0).gameObject.SetActive(activeEquipment);
        }
    }
}
