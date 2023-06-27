using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSceneUI : SceneUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["QuestButton"].onClick.AddListener(() => { Player.Instance.questUI.OpenQuest(); });
        buttons["InventoryButton"].onClick.AddListener(() => { Player.Instance.inventoryUI.OpenInventory();  });
        buttons["EquipmentButton"].onClick.AddListener(() => { Player.Instance.equipUI.OpenEquipment(); });
        buttons["SettingButton"].onClick.AddListener(() => { GameManager.Ui.ShowPopUpUI<SettingPopupUI>("UI/Setting"); });
    }
}
