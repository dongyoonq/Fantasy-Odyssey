using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSceneUI : SceneUI
{
    InventoryPopupUI inventoryUI;

    protected override void Awake()
    {
        base.Awake();


        buttons["InventoryButton"].onClick.AddListener(() => { GameManager.Ui.ShowPopUpUI<InventoryPopupUI>("UI/Inventory"); });
        /*{
            if (inventoryUI != GameManager.Inventory.ui)
            {
                inventoryUI = GameManager.Ui.ShowPopUpUI(GameManager.Inventory.ui);
                GameManager.Inventory.ui = inventoryUI;
            }

            GameManager.Ui.ShowPopUpUI(GameManager.Inventory.ui); 
        });*/
        buttons["SettingButton"].onClick.AddListener(() => { GameManager.Ui.ShowPopUpUI<SettingPopupUI>("UI/Setting"); });
    }
}
