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

    public List<ItemData> list;

    private void Start()
    {
        Player.Instance.OnAddItemInventory.AddListener(AddInventory);
        Player.Instance.OnRemoveItemInventory.AddListener(RemoveInventory);
        SlotCnt = 30;
        list = new List<ItemData>(SlotCnt);
        for (int i = 0; i < SlotCnt; i++)
            list.Add(null);
    }

    void AddInventory(ItemData itemData, int index)
    {
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(true);
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = itemData.sprite;
        Player.Instance.inventoryUI.slots[index].data = itemData;
    }

    void RemoveInventory(ItemData itemData, int index)
    {
        Debug.Log(index);
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(false);
        Player.Instance.inventoryUI.slots[index].data = null;
    }
}