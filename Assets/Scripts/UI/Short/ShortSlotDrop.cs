using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShortSlotDrop : MonoBehaviour, IDropHandler
{
    public static bool isSuccessMove = false;

    public void OnDrop(PointerEventData eventData)
    {
        AddSlot();
        MoveSlot();
    }

    void AddSlot()
    {
        if (InventorySlotDrag.draggingItem == null)
            return;

        if (InventorySlotDrag.draggingItem.transform.parent.GetComponent<InventorySlot>().data.prefab is not IUsable)
            return;

        for (int i = 0; i < Player.Instance.shortUI.slots.Length; i++)
        {
            if (Player.Instance.shortUI.slots[i].usableItem == InventorySlotDrag.draggingItem.transform.parent.GetComponent<InventorySlot>().data)
            {
                Player.Instance.shortUI.slots[i].usableItem = null;
                Player.Instance.shortUI.slots[i].amount = 0;
                Player.Instance.shortUI.slots[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                Player.Instance.shortUI.slots[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                Player.Instance.shortUI.slots[i].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "0";
                break;
            }
        }

        GetComponent<ShortSlot>().usableItem = InventorySlotDrag.draggingItem.transform.parent.GetComponent<InventorySlot>().data;
        GetComponent<ShortSlot>().amount = InventorySlotDrag.draggingItem.transform.parent.GetComponent<InventorySlot>().amount;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = InventorySlotDrag.draggingItem.GetComponent<Image>().sprite;
        transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = GetComponent<ShortSlot>().amount.ToString();
    }

    void MoveSlot()
    {
        if (ShortSlotDrag.draggingItem == null)
            return;

        if (GetComponent<ShortSlot>().usableItem == ShortSlotDrag.draggingItem.transform.parent.parent.GetComponent<ShortSlot>().usableItem)
        {
            ShortSlotDrag.draggingItem.SetActive(true);
            ShortSlotDrag.draggingItem = null;
            return;
        }

        GameObject start = gameObject;
        GameObject target = ShortSlotDrag.draggingItem.transform.parent.parent.gameObject;

        start.GetComponent<ShortSlot>().usableItem = target.GetComponent<ShortSlot>().usableItem;
        start.GetComponent<ShortSlot>().amount = target.GetComponent<ShortSlot>().amount;
        start.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ShortSlotDrag.draggingItem.gameObject.GetComponent<Image>().sprite;
        start.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = start.GetComponent<ShortSlot>().amount.ToString();
        start.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        target.GetComponent<ShortSlot>().usableItem = null;
        target.GetComponent<ShortSlot>().amount = 0;
        target.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
        target.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = target.GetComponent<ShortSlot>().amount.ToString();
        target.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        isSuccessMove = true;
    }
}
