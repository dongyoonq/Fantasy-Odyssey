using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = 100000000)]
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
    protected string itemName;
    protected int price;

    public string Tooltip => _tooltip;

    [SerializeField] private string _tooltip; // 아이템 설명

    public Item prefab; 

    /// <summary> 타입에 맞는 새로운 아이템 생성 </summary>
    public abstract Item CreateItem();
}
