using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpUI : BaseUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void CloseUI()
    {
        GameManager.Ui.ClosePopUpUI();
    }
}
