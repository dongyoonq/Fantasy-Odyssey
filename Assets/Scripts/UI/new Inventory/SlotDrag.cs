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

    void OnMouseDoubleClick()
    {
        if (transform.GetComponent<Slot>().data == null)
            return;

        Equipment equip = Instantiate(transform.GetComponent<Slot>().data.prefab) as Equipment;

        equip.Data = transform.GetComponent<Slot>().data;
        Player.Instance.OnEquip(equip, transform.GetComponent<Slot>().slotIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetChild(0).IsValid())
        {
            // 드래그시에 복사본 생성
            target = Instantiate(transform.GetChild(0));
            target.SetParent(GameObject.Find("InventoryUI").transform);
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
        if (target.IsValid())
        {
            Destroy(target.gameObject);
            target.localPosition = Vector3.zero;

            target.GetComponent<Image>().raycastTarget = true;
        }

        if(SlotDrop.swapItemIsActiveObj)
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
}
