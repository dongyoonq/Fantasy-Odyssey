using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDrag : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    float clickTime = 0;

    public static GameObject draggingItem = null;
    Transform target;
    Rect baseRect;

    void Start()
    {
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
    }

    void OnMouseDoubleClick()
    {
        if (transform.GetComponent<Slot>().data == null)
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

            //if (GetComponent<Slot>().data is not CountableItemData)
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
        if (draggingItem != null && GetComponent<Slot>().data != null)
        {
            if (target.localPosition.x < baseRect.xMin
            || target.localPosition.x > baseRect.xMax
            || target.localPosition.y < baseRect.yMin
            || target.localPosition.y > baseRect.yMax)
            {
                Item fieldItem = Instantiate(GetComponent<Slot>().data.prefab, Player.Instance.foot.position, Quaternion.Euler(-30f,-45f,0));
                ItemData tempData = GetComponent<Slot>().data;
                Player.Instance.RemoveItemFromInventory(GetComponent<Slot>().data, GetComponent<Slot>().slotIndex);
                fieldItem.AddComponent<FieldItem>();
                fieldItem.GetComponent<FieldItem>().itemData = tempData;

                if (draggingItem.transform.parent.GetComponent<Slot>().data is CountableItemData)
                    if (draggingItem.transform.parent.GetComponent<Slot>().amount != 0)
                        transform.GetChild(0).gameObject.SetActive(true);

                Destroy(target.gameObject);
                SlotDrop.swapItemIsActiveObj = true;
                draggingItem = null;
                return;
            }
        }

        if (target.IsValid())
            Destroy(target.gameObject);

        if (SlotDrop.swapItemIsActiveObj)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);

        if (draggingItem == null && transform.GetComponent<Slot>().data == null)
            transform.GetChild(0).gameObject.SetActive(false);

        SlotDrop.swapItemIsActiveObj = true;
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
        ItemData data = GetComponent<Slot>().data;

        if (data is EquipmentData)
        {
            Equipment equip = Instantiate(data.prefab) as Equipment;
            equip.Data = transform.GetComponent<Slot>().data;
            Player.Instance.OnEquip(equip, transform.GetComponent<Slot>().slotIndex);
        }
        else if (data is CountableItemData)
        {
            if (data.prefab is IUsable)
                Player.Instance.useItem(data, GetComponent<Slot>().slotIndex);
        }
    }
}
