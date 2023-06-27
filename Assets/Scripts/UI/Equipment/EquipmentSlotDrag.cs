using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotDrag : MonoBehaviour, IPointerClickHandler
{
    float clickTime = 0;

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

    void OnMouseDoubleClick()
    {
        if (transform.GetComponent<EquipmentSlot>().data == null)
            return;

        Equipment equip = Instantiate(GetComponent<EquipmentSlot>().data.prefab) as Equipment;
        equip.Data = transform.GetComponent<EquipmentSlot>().data;
        transform.GetComponent<EquipmentSlot>().data = null; 
        Player.Instance.UnEquip(equip);
    }
}
