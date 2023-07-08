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
    public bool activeInventory = false;

    public InventorySlot[] slots;

    Vector2 orgPosition;

    private void Start()
    {
        inventory = Player.Instance.inventory;
        Player.Instance.inventoryUI = this;
        inventory.onSlotCountChange.AddListener(SlotChange);
        slots = inventoryPanel.transform.GetChild(0).GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
            slots[i].slotIndex = i;

        inventoryPanel.transform.GetChild(0).gameObject.SetActive(activeInventory);

        inventoryPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() => { OpenInventory(); GameManager.Sound.PlaySFX("Click"); });
        inventory.onChangeInventory.AddListener(() => InventoryChange());

        orgPosition = inventoryPanel.transform.GetChild(0).position;
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
        inventoryPanel.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"<color=#F8913F>{cnt}</color> / 30".ToString();
    }


    public void OpenInventory()
    {
        GameManager.Sound.PlaySFX("OpenUI");
        activeInventory = !activeInventory;
        inventoryPanel.transform.GetChild(0).gameObject.SetActive(activeInventory);
        inventoryPanel.transform.GetChild(0).position = orgPosition;
        inventoryPanel.transform.SetAsLastSibling();
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Sound.PlaySFX("OpenUI");
            activeInventory = !activeInventory;
            inventoryPanel.transform.GetChild(0).gameObject.SetActive(activeInventory);
            inventoryPanel.transform.GetChild(0).position = orgPosition;
            inventoryPanel.transform.SetAsLastSibling();
        }
    }

    public void AddSlot()
    {
        inventory.SlotCnt++;
    }
}
