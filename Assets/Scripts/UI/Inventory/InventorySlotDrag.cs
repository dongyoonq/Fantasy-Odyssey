using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDrag : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    float clickTime = 0;

    public static GameObject draggingItem = null;
    Transform target;
    Rect baseRect;
    Rect shortSlotRect;

    GameObject shortSlotPanel;

    void Start()
    {
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        shortSlotPanel = GameObject.Find("ShortUI");
        shortSlotRect = shortSlotPanel.GetComponent<RectTransform>().rect;
    }

    void OnMouseDoubleClick()
    {
        if (transform.GetComponent<InventorySlot>().data == null)
            return;

        SlotUse();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetChild(0).IsValid())
        {
            // 드래그시에 복사본 생성
            target = Instantiate(transform.GetChild(0));
            target.SetParent(GameObject.Find("InventoryUI").transform);

            //if (GetComponent<InventorySlot>().data is not CountableItemData)
            transform.GetChild(0).gameObject.SetActive(false);

            target.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (target.IsValid())
        {
            target.position = eventData.position;
            draggingItem = transform.GetChild(0).gameObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingItem != null && GetComponent<InventorySlot>().data != null && ShortSlotDrop.isShortSlotDrop)
        {
            if (target.position.x > shortSlotPanel.transform.position.x - (shortSlotRect.width / 2)
                && target.position.x < shortSlotPanel.transform.position.x + (shortSlotRect.width / 2)
                && target.position.y > shortSlotPanel.transform.position.y - (shortSlotRect.height / 2)
                && target.position.y < shortSlotPanel.transform.position.y + (shortSlotRect.height / 2))
            {
                transform.GetChild(0).gameObject.SetActive(true);
                Destroy(target.gameObject);
                draggingItem = null;
                ShortSlotDrop.isShortSlotDrop = false;
                return;
            }
        }

        if (draggingItem != null && GetComponent<InventorySlot>().data != null && !ShortSlotDrop.isShortSlotDrop)
        {
            if (target.position.x < transform.parent.parent.position.x - (baseRect.width / 2)
                || target.position.x > transform.parent.parent.position.x + (baseRect.width / 2)
                || target.position.y < transform.parent.parent.position.y - (baseRect.height / 2)
                || target.position.y > transform.parent.parent.position.y + (baseRect.height / 2))
            {
                Item fieldItem = Instantiate(GetComponent<InventorySlot>().data.prefab, Player.Instance.foot.position, Quaternion.Euler(-30f,-45f,0));
                ItemData tempData = GetComponent<InventorySlot>().data;
                Player.Instance.RemoveItemFromInventory(GetComponent<InventorySlot>().data, GetComponent<InventorySlot>().slotIndex);
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;

                if (draggingItem.transform.parent.GetComponent<InventorySlot>().data is CountableItemData)
                    if (draggingItem.transform.parent.GetComponent<InventorySlot>().amount != 0)
                        transform.GetChild(0).gameObject.SetActive(true);

                Destroy(target.gameObject);
                InventorySlotDrop.swapItemIsActiveObj = true;
                draggingItem = null;
                return;
            }
        }

        if (target.IsValid())
            Destroy(target.gameObject);

        if (InventorySlotDrop.swapItemIsActiveObj)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);

        if (draggingItem == null && transform.GetComponent<InventorySlot>().data == null)
            transform.GetChild(0).gameObject.SetActive(false);

        InventorySlotDrop.swapItemIsActiveObj = true;
        draggingItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - clickTime) < 0.3f)
        {
            OnMouseDoubleClick();
            clickTime = -1;
        }
        else
            clickTime = Time.time;
    }

    void SlotUse()
    {
        ItemData data = GetComponent<InventorySlot>().data;

        if (data is EquipmentData)
        {
            Equipment equip = Instantiate(data.prefab) as Equipment;
            equip.Data = transform.GetComponent<InventorySlot>().data;
            Player.Instance.OnEquip(equip, transform.GetComponent<InventorySlot>().slotIndex);
        }
        else if (data is CountableItemData)
        {
            if (data.prefab is IUsable)
                Player.Instance.useItem(data, GetComponent<InventorySlot>().slotIndex);
        }
    }
}
