using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHightLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Sprite highLightImg;
    Sprite orgImg;

    void Start()
    {
        orgImg = GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = highLightImg;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = orgImg;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Slot>().data == null) return;

        transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GetComponent<Slot>().data == null) return;

        transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }
}
