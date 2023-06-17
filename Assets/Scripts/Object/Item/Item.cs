using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum ItemType
    {
        Equipment,
        Usable,
        Etc,
    }

    protected ItemType Type;
    protected string itemName;
    protected int price;

    public abstract void Use();
}
