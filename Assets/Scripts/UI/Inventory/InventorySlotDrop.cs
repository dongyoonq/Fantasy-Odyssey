using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDrop : MonoBehaviour, IDropHandler
{
    public static bool swapItemIsActiveObj = true;

    public void OnDrop(PointerEventData eventData)
    {
        Swap();
    }

    void Swap()
    {
        if (InventorySlotDrag.draggingItem == null)
            return;

        if (!transform.GetChild(0).IsValid())
            swapItemIsActiveObj = false;

        if (GetComponent<InventorySlot>().data == InventorySlotDrag.draggingItem.transform.parent.GetComponent<InventorySlot>().data)
        {
            InventorySlotDrag.draggingItem.SetActive(true); 
            swapItemIsActiveObj = true;
            InventorySlotDrag.draggingItem = null;
            return;
        }

        Transform start = transform.GetChild(0);
        Transform target = InventorySlotDrag.draggingItem.transform;
        
        Transform tempParent = start.parent;

        start.SetParent(target.parent);
        start.localPosition = Vector2.zero;
        target.SetParent(tempParent);
        target.localPosition = Vector2.zero;

        start.SetAsFirstSibling();
        target.SetAsFirstSibling();

        // 바뀌고 난뒤

        // dataSwap
        ItemData tmpData = start.parent.GetComponent<InventorySlot>().data;
        int tmpAmount = start.parent.GetComponent<InventorySlot>().amount;
        int indexA = Player.Instance.inventoryUI.slots.ToList().FindIndex(x => x == start.parent.GetComponent<InventorySlot>());
        int indexB = Player.Instance.inventoryUI.slots.ToList().FindIndex(x => x == target.parent.GetComponent<InventorySlot>());

        if (!swapItemIsActiveObj)
        {
            start.gameObject.SetActive(false);
            target.gameObject.SetActive(true);

            //int index = Player.Instance.inventory.list.FindIndex(x => x == start.parent.GetComponent<InventorySlot>().data);
            Player.Instance.inventory.list[indexA] = null;
            Player.Instance.inventory.list[indexB] = tmpData;

            start.parent.GetComponent<InventorySlot>().data = null;
            target.parent.GetComponent<InventorySlot>().data = tmpData;

        }
        else
        {
            start.gameObject.SetActive(true);
            target.gameObject.SetActive(true);

            Player.Instance.inventory.list[indexA] = Player.Instance.inventory.list[indexB];
            Player.Instance.inventory.list[indexB] = tmpData;

            start.parent.GetComponent<InventorySlot>().data = target.parent.GetComponent<InventorySlot>().data;
            target.parent.GetComponent<InventorySlot>().data = tmpData;
        }

        start.parent.GetComponent<InventorySlot>().amount = target.parent.GetComponent<InventorySlot>().amount;
        target.parent.GetComponent<InventorySlot>().amount = tmpAmount;

        start.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"<color=#D76A2E>{start.parent.GetComponent<InventorySlot>().amount}</color>";
        target.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"<color=#D76A2E>{target.parent.GetComponent<InventorySlot>().amount}</color>";

        InventorySlotDrag.draggingItem = null;
    }
}
