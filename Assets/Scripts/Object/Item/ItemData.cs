using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Usable,
        Etc,
    }

    public Sprite sprite;
    protected ItemType Type;
    public string itemName;
    protected int price;

    public string Tooltip => _tooltip;

    [SerializeField] private string _tooltip; // 아이템 설명

    public Item prefab;
}
