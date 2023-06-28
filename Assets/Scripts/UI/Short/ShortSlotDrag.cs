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
        if (draggingItem == null)
            return;

        if (draggingItem != null && GetComponent<ShortSlot>().usableItem != null)
        {
            if (target.localPosition.x < baseRect.xMin
                || target.localPosition.x > baseRect.xMax
                || target.localPosition.y < baseRect.yMin
                || target.localPosition.y > baseRect.yMax)
            {
                draggingItem.transform.parent.parent.GetComponent<ShortSlot>().usableItem = null;
                draggingItem.transform.parent.parent.GetComponent<ShortSlot>().amount = 0;
                draggingItem.SetActive(false);
                draggingItem.GetComponent<Image>().sprite = null;
                draggingItem.transform.parent.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "0";

                Destroy(target.gameObject);
                draggingItem = null;
                return;
            }
        }

        draggingItem.SetActive(true);
        Destroy(target.gameObject);

        if (ShortSlotDrop.isSuccessMove)
        {
            ShortSlotDrop.isSuccessMove = false;
            draggingItem.SetActive(false);
        }

        draggingItem = null;
    }
}
