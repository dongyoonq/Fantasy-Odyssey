using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShortSlotUI : SceneUI
{
    public GameObject shortPanel;

    public ShortSlot[] slots;

    private void Start()
    {
        Player.Instance.shortUI = this;
        slots = shortPanel.transform.GetChild(0).GetComponentsInChildren<ShortSlot>();
        Player.Instance.OnChangeShortSlot.AddListener(UpdateSlot);
    }

    void UpdateSlot(ItemData data)
    {
        int indexA = Array.FindIndex(slots, x => x.usableItem == data);
        int indexB = Player.Instance.inventory.list.FindIndex(x => x == data);

        if (indexA == -1)
            return;

        if (indexB == -1 || Player.Instance.inventoryUI.slots[indexB].data == null)
        {
            slots[indexA].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

            slots[indexA].usableItem = null;
            slots[indexA].amount = 0;

            slots[indexA].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
            slots[indexA].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "0";
            return;
        }

        slots[indexA].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        slots[indexA].usableItem = Player.Instance.inventoryUI.slots[indexB].data;
        slots[indexA].amount = Player.Instance.inventoryUI.slots[indexB].amount;

        slots[indexA].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Player.Instance.inventoryUI.slots[indexB].data.sprite;
        slots[indexA].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = slots[indexA].amount.ToString();
    }
}
