using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 수량 아이템 - 포션 아이템 </summary>
public class EtcItem : CountableItem
{
    [SerializeField] protected EtcItemData etcItemData;

    public EtcItem(EtcItemData data) : base(data) { }
}