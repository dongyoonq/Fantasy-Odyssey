using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDrop : MonoBehaviour, IDropHandler
{
    public static bool swapItemIsActiveObj = true;

    public void OnDrop(PointerEventData eventData)
    {
        Swap();
    }

    void Swap()
    {
        if (SlotDrag.draggingItem == null)
            return;

        if (!transform.GetChild(0).IsValid())
            swapItemIsActiveObj = false;
        
        Transform start = transform.GetChild(0);
        Transform target = SlotDrag.draggingItem.transform;
        
        Transform tempParent = start.parent;

        start.SetParent(target.parent);
        start.localPosition = Vector2.zero;
        target.SetParent(tempParent);
        target.localPosition = Vector2.zero;

        // ¹Ù²î°í ³­µÚ

        // dataSwap
        ItemData tmpData = start.parent.GetComponent<Slot>().data;
        int indexA = Player.Instance.inventoryUI.slots.ToList().FindIndex(x => x == start.parent.GetComponent<Slot>());
        int indexB = Player.Instance.inventoryUI.slots.ToList().FindIndex(x => x == target.parent.GetComponent<Slot>());

        if (!swapItemIsActiveObj)
        {
            start.gameObject.SetActive(false);
            target.gameObject.SetActive(true);

            //int index = Player.Instance.inventory.list.FindIndex(x => x == start.parent.GetComponent<Slot>().data);
            Player.Instance.inventory.list[indexA] = null;
            Player.Instance.inventory.list[indexB] = tmpData;

            start.parent.GetComponent<Slot>().data = null;
            target.parent.GetComponent<Slot>().data = tmpData;

        }
        else
        {
            start.gameObject.SetActive(true);
            target.gameObject.SetActive(true);

            Player.Instance.inventory.list[indexA] = Player.Instance.inventory.list[indexB];
            Player.Instance.inventory.list[indexB] = tmpData;

            start.parent.GetComponent<Slot>().data = target.parent.GetComponent<Slot>().data;
            target.parent.GetComponent<Slot>().data = tmpData;
        }

        SlotDrag.draggingItem = null;

    }
}
