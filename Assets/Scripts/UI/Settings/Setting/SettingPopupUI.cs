using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopupUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["Button_Close"].onClick.AddListener(() => { base.CloseUI(); });
    }
}