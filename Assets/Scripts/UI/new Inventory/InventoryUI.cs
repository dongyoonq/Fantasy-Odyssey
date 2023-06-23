using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

        for (int i = 0; i < slots.Length; i++)
            slots[i].slotIndex = i;

        if (inventoryPanel.IsValid())
            inventoryPanel.SetActive(activeInventory);

        inventoryPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { OpenInventory(); });
        inventory.onChangeInvntory.AddListener(() => InventoryChange());
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

    private void InventoryChange()
    {
        int cnt = inventory.list.Count(x => x != null);
        inventoryPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"<color=#F8913F>{cnt}</color> / 30".ToString();
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
