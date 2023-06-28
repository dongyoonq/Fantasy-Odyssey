using UnityEngine;
/// <summary> 셀 수 있는 아이템 데이터 </summary>
public abstract class CountableItemData : ItemData
{
    public int MaxAmount { get { return _maxAmount; } set { _maxAmount = value; } }
    [SerializeField] private int _maxAmount = 99;
}