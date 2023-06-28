using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ - ���� ������ </summary>
public class Potion : CountableItem, IUsable
{
    [SerializeField] protected PotionData portionData;

    public Potion(PotionData data) : base(data) { }

    public virtual void Use() {  }
}