using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSceneUI : SceneUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["StatusButton"].onClick.AddListener(() => { Player.Instance.statusUI.OpenStatus(); GameManager.Sound.PlaySFX("Click"); });
        buttons["QuestButton"].onClick.AddListener(() => { Player.Instance.questUI.OpenQuest(); GameManager.Sound.PlaySFX("Click"); } );
        buttons["InventoryButton"].onClick.AddListener(() => { Player.Instance.inventoryUI.OpenInventory(); GameManager.Sound.PlaySFX("Click"); });
        buttons["EquipmentButton"].onClick.AddListener(() => { Player.Instance.equipUI.OpenEquipment(); GameManager.Sound.PlaySFX("Click"); });
        buttons["SettingButton"].onClick.AddListener(() => { GameManager.Ui.ShowPopUpUI<SettingPopupUI>("UI/Setting"); GameManager.Sound.PlaySFX("OpenUI"); GameManager.Sound.PlaySFX("Click"); });
    }
}
