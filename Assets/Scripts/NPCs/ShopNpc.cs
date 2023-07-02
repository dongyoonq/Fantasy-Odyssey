using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopNpc : MonoBehaviour
{
    [SerializeField] List<ItemData> shopItemList;
    ShopUI shopUI;

    public void OpenShop()
    {
        Player.Instance.GetComponent<PlayerInput>().enabled = false;
        shopUI = GameManager.Resource.Instantiate<ShopUI>("UI/Shop");
        shopUI.transform.SetParent(GameObject.Find("SceneUI").transform, false);

        foreach (ItemData item in shopItemList)
        {
            ShopSlot slot = GameManager.Resource.Instantiate<ShopSlot>("UI/ShopSlot", shopUI.content);
            slot.itemData = item;
            slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = slot.itemData.sprite;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = slot.itemData.itemName;
            slot.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ShopSell(slot.itemData));
            slot.detail = shopUI.detail;
        }

        shopUI.closeButton.onClick.AddListener(CloseShop);
    }

    public void CloseShop()
    {
        Player.Instance.GetComponent<PlayerInput>().enabled = true;
        GameManager.Resource.Destroy(shopUI.gameObject);
        shopUI.closeButton.onClick.RemoveAllListeners();
    }

    public void ShopSell(ItemData itemData)
    {
        Player.Instance.AddItemToInventory(itemData);
    }
}