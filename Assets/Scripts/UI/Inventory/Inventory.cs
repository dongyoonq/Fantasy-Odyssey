using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public UnityEvent<int> onSlotCountChange;
    public UnityEvent onChangeInventory;

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

    void AddInventory(ItemData itemData, int index, int amount)
    {
        if (itemData is CountableItemData)
            Player.Instance.inventoryUI.slots[index].amount++;
        else
            Player.Instance.inventoryUI.slots[index].amount = 1;

        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(true);
        Player.Instance.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = itemData.sprite;
        Player.Instance.inventoryUI.slots[index].data = itemData;

        //<color=#D76A2E>0</color>
        Player.Instance.inventoryUI.slots[index].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"<color=#D76A2E>{Player.Instance.inventoryUI.slots[index].amount}</color>";


        onChangeInventory?.Invoke();
    }

    void RemoveInventory(ItemData itemData, int index, int amount)
    {
        if (itemData is CountableItemData)
        {
            Player.Instance.inventoryUI.slots[index].amount--;

            if (Player.Instance.inventoryUI.slots[index].amount == 0)
            {
                Player.Instance.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
                Player.Instance.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(false);

                Player.Instance.inventoryUI.slots[index].data = null;
            }
        }
        else
        {
            Player.Instance.inventoryUI.slots[index].amount = 0;

            Player.Instance.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
            Player.Instance.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(false);

            Player.Instance.inventoryUI.slots[index].data = null;
        }

        Player.Instance.inventoryUI.slots[index].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"<color=#D76A2E>{Player.Instance.inventoryUI.slots[index].amount}</color>";

        onChangeInventory?.Invoke();
    }
}