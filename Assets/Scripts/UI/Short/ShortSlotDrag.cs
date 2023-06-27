using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShortSlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject draggingitem;
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
            draggingitem = transform.GetChild(0).GetChild(0).gameObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingitem == null)
            return;

        if (draggingitem != null && GetComponent<ShortSlot>().usableItem != null)
        {
            if (target.localPosition.x < baseRect.xMin
                || target.localPosition.x > baseRect.xMax
                || target.localPosition.y < baseRect.yMin
                || target.localPosition.y > baseRect.yMax)
            {
                draggingitem.transform.parent.parent.GetComponent<ShortSlot>().usableItem = null;
                draggingitem.transform.parent.parent.GetComponent<ShortSlot>().amount = 0;
                draggingitem.SetActive(false);
                draggingitem.GetComponent<Image>().sprite = null;
                draggingitem.transform.parent.parent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "0";

                Destroy(target.gameObject);
                draggingitem = null;
                return;
            }
        }

        draggingitem.SetActive(true);
        Destroy(target.gameObject);
        draggingitem = null;
    }
}
