using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlotHightLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Sprite highLightImg;
    Sprite orgImg;
    RectTransform detail;

    void Start()
    {
        orgImg = GetComponent<Image>().sprite;
        detail = GameObject.Find("EquipmentUI").transform.GetChild(1).GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = highLightImg;

        if (GetComponent<EquipmentSlot>().data != null)
        {
            detail.gameObject.SetActive(true);
            GameObject itemInfo = detail.GetChild(0).GetChild(0).gameObject;
            itemInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetComponent<EquipmentSlot>().data.sprite;
            itemInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = GetComponent<EquipmentSlot>().data.itemName;
            itemInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = GetComponent<EquipmentSlot>().data.Tooltip;

            Rect deatilRect = detail.GetComponent<RectTransform>().rect;
            if (eventData.position.y > Screen.height / 2 && eventData.position.x > Screen.width / 2)
                detail.position = new Vector2(eventData.position.x - deatilRect.xMax - 15, eventData.position.y - deatilRect.yMax - 15);
            else if (eventData.position.y < Screen.height / 2 && eventData.position.x > Screen.width / 2)
                detail.position = new Vector2(eventData.position.x - deatilRect.xMax - 15, eventData.position.y + deatilRect.yMax + 15);
            else if (eventData.position.y > Screen.height / 2 && eventData.position.x < Screen.width / 2)
                detail.position = new Vector2(eventData.position.x + deatilRect.xMax + 15, eventData.position.y - deatilRect.yMax - 15);
            else if (eventData.position.y < Screen.height / 2 && eventData.position.x < Screen.width / 2)
                detail.position = new Vector2(eventData.position.x + deatilRect.xMax + 15, eventData.position.y + deatilRect.yMax + 15);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = orgImg;

        detail.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<EquipmentSlot>().data == null) return;

        transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GetComponent<EquipmentSlot>().data == null) return;

        transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }
}
