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
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}