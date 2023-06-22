using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["Close Button"].onClick.AddListener(() => { base.CloseUI(); });
    }
}