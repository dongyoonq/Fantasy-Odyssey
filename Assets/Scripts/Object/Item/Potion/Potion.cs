using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 수량 아이템 - 포션 아이템 </summary>
public class Potion : CountableItem, IUsable
{
    [SerializeField] protected PotionData potionData;

    public Potion(PotionData data) : base(data) { }

    public virtual void Use() {  }
}