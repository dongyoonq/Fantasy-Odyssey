using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData Data { get; set; }

    public Item(ItemData data) => Data = data;
}