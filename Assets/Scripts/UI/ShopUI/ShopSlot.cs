using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Transform detail;

    public ItemData itemData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject itemInfo = detail.GetChild(0).gameObject;
        itemInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = itemData.itemName;
        itemInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = itemData.Tooltip;
        itemInfo.transform.GetChild(2).gameObject.SetActive(false);

        StringBuilder applyStat = new StringBuilder();

        if (itemData is ArmorData armorData)
        {
            itemInfo.transform.GetChild(2).gameObject.SetActive(true);
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
            itemInfo.transform.GetChild(2).gameObject.SetActive(true);
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

        itemInfo.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = applyStat.ToString();
        itemInfo.transform.GetChild(2).GetChild(1).GetComponent<Scrollbar>().value = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}