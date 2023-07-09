using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            ItemData itemData = GetComponent<EquipmentSlot>().data;

            detail.gameObject.SetActive(true);
            GameObject itemInfo = detail.GetChild(0).gameObject;
            itemInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetComponent<EquipmentSlot>().data.sprite;
            itemInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = GetComponent<EquipmentSlot>().data.itemName;
            itemInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = GetComponent<EquipmentSlot>().data.Tooltip;

            StringBuilder applyStat = new StringBuilder();

            if (itemData is ArmorData armorData)
            {
                applyStat.Append("<color=#86D946>적용 스텟</color>\n");
                if (armorData.MaxHP != 0)
                    applyStat.Append($"MaxHP <color=#00D2FF>+{armorData.MaxHP}</color>\n");
                if (armorData.Deffense != 0)
                    applyStat.Append($"Deffense <color=#00D2FF>+{armorData.Deffense}</color>\n");
                if (armorData.WalkSpeed != 0)
                    applyStat.Append($"WalkSpeed <color=#00D2FF>+{armorData.WalkSpeed}</color>\n");
                if (armorData.RunSpeed != 0)
                    applyStat.Append($"RunSpeed <color=#00D2FF>+{armorData.RunSpeed}</color>\n");
                if (armorData.JumpPower != 0)
                    applyStat.Append($"JumpPower <color=#00D2FF>+{armorData.JumpPower}</color>\n");
            }
            else if (itemData is WeaponData weaponData)
            {
                applyStat.Append("<color=#86D946>적용 스텟</color>\n");
                if (weaponData.AttackPower != 0)
                    applyStat.Append($"AttackPower <color=#00D2FF>+{weaponData.AttackPower}</color>\n");
                if (weaponData.AttackSpeed != 0)
                    applyStat.Append($"AttackSpeed : <color=#00D2FF>{weaponData.AttackSpeed}</color>\n");
                if (weaponData.MaxCombo != 0)
                    applyStat.Append($"MaxCombo : <color=#00D2FF>{weaponData.MaxCombo}</color>\n");
                if (weaponData.CoolTimeSkill != 0)
                    applyStat.Append($"UltSkillCoolTime : <color=#00D2FF>{weaponData.CoolTimeSkill}</color>\n");
            }

            itemInfo.transform.GetChild(3).GetComponent<TMP_Text>().text = applyStat.ToString();

            Rect deatilRect = detail.GetComponent<RectTransform>().rect;
            if (eventData.position.y > Screen.height / 2 && eventData.position.x > Screen.width / 2)
                detail.position = new Vector2(eventData.position.x - (deatilRect.width / 2), eventData.position.y - (deatilRect.height / 2));
            else if (eventData.position.y < Screen.height / 2 && eventData.position.x > Screen.width / 2)
                detail.position = new Vector2(eventData.position.x - (deatilRect.width / 2), eventData.position.y + (deatilRect.height / 2));
            else if (eventData.position.y > Screen.height / 2 && eventData.position.x < Screen.width / 2)
                detail.position = new Vector2(eventData.position.x + (deatilRect.width / 2), eventData.position.y - (deatilRect.height / 2));
            else if (eventData.position.y < Screen.height / 2 && eventData.position.x < Screen.width / 2)
                detail.position = new Vector2(eventData.position.x + (deatilRect.width / 2), eventData.position.y + (deatilRect.height / 2));
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
