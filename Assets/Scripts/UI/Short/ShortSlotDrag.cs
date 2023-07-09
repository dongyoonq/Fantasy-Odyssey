using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShortSlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggingItem;
    Transform target;
    Rect baseRect;

    private void Start()
    {
        baseRect = transform.parent.GetComponent<RectTransform>().rect;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetChild(0).IsValid())
        {
            // 드래그시에 복사본 생성
            target = Instantiate(transform.GetChild(0).GetChild(0));
            target.SetParent(GameObject.Find("ShortUI").transform);

            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

            target.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (target.IsValid())
        {
            target.position = eventData.position;
            draggingItem = transform.GetChild(0).GetChild(0).gameObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingItem != null && GetComponent<ShortSlot>().usableItem != null)
        {
            if (target.position.x < transform.parent.position.x - (baseRect.width / 2)
                || target.position.x > transform.parent.position.x + (baseRect.width / 2)
                || target.position.y < transform.parent.position.y - (baseRect.height / 2)
                || target.position.y > transform.parent.position.y + (baseRect.height / 2))
            {
                draggingItem.transform.parent.parent.GetComponent<ShortSlot>().usableItem = null;
                draggingItem.transform.parent.parent.GetComponent<ShortSlot>().amount = 0;
                draggingItem.SetActive(false);
                draggingItem.GetComponent<Image>().sprite = null;
                draggingItem.transform.parent.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "0";

                ShortSlotDrop.isShortSlotDrop = false;
                Destroy(target.gameObject);
                draggingItem = null;
                return;
            }
        }

        if (target.IsValid())
            Destroy(target.gameObject);

        if (ShortSlotDrop.isSuccessMove)
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        ShortSlotDrop.isSuccessMove = false;
        ShortSlotDrop.isShortSlotDrop = false;
        draggingItem = null;
    }
}
