using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcItem : CountableItem
{
    [SerializeField] protected EtcItemData etcItemData;

    public EtcItem(EtcItemData data) : base(data) { }
}