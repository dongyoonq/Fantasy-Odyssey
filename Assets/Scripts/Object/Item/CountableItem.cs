using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 수량을 셀 수 있는 아이템 </summary>
public abstract class CountableItem : Item
{
    public CountableItemData CountableData { get; private set; }

    public CountableItem(CountableItemData data) : base(data)
    {
        CountableData = data;
    }
}