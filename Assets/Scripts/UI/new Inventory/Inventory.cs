using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public UnityEvent<int> onSlotCountChange;

    private int slotCnt;

    public int SlotCnt 
    { 
        get => slotCnt;
        set { slotCnt = value; onSlotCountChange?.Invoke(slotCnt); }

    }

    /////////////////////////////////////////////////////

    public List<Item> list;

    private void Start()
    {
        Player.Instance.OnAddItemInventory.AddListener(AddInventory);
        SlotCnt = 30;
        list = new List<Item>(SlotCnt);
    }

    void AddInventory(Item item, int index)
    {
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(true);
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = item.Data.sprite;
    }
}