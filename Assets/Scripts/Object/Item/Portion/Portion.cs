using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ - ���� ������ </summary>
public class Portion : CountableItem, IUsable
{
    [SerializeField] protected PortionData portionData;

    public Portion(PortionData data) : base(data) { }

    public virtual void Use() {  }
}