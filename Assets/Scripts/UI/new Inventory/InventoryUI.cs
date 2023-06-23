using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    Inventory inventory;

    public GameObject inventoryPanel;
    bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inventory = Player.Instance.inventory;
        Player.Instance.inventoryUI = this;
        inventory.onSlotCountChange.AddListener(SlotChange);
        slots = GetComponentsInChildren<Slot>();
        if (inventoryPanel.IsValid())
            inventoryPanel.SetActive(activeInventory);

        Button btn = inventoryPanel.transform.GetChild(0).GetComponent<Button>();
        btn.onClick.AddListener(() => { OpenInventory(); });
    }

    private void SlotChange(int val)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void OpenInventory()
    {
        if (!activeInventory)
            GameManager.Ui.popUpUIStack.Push(this);
        else
            GameManager.Ui.popUpUIStack.Pop();

        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!activeInventory)
                GameManager.Ui.popUpUIStack.Push(this);
            else
                GameManager.Ui.popUpUIStack.Pop();

            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }

    public void AddSlot()
    {
        inventory.SlotCnt++;
    }
}
