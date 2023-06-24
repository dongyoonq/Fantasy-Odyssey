using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 수량 아이템 - 포션 아이템 </summary>
public class Portion : CountableItem, IUsable
{
    [SerializeField] protected PortionData portionData;

    public Portion(PortionData data) : base(data) { }

    public virtual void Use() {  }
}