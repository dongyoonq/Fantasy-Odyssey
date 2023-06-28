using UnityEngine;
/// <summary> �� �� �ִ� ������ ������ </summary>
public abstract class CountableItemData : ItemData
{
    public int MaxAmount { get { return _maxAmount; } set { _maxAmount = value; } }
    [SerializeField] private int _maxAmount = 99;
}