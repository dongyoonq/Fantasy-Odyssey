using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestSlotEquip : MonoBehaviour, IPointerClickHandler
{
    float clickTime = 0;

    void OnMouseDoubleClick()
    {
        Equipment equip = Instantiate(transform.GetComponent<Slot>().data.prefab) as Equipment;
        equip.Data = transform.GetComponent<Slot>().data;
        Player.Instance.OnEquip(equip);
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
