using UnityEngine;
/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Portion Data", menuName = "Scriptable Object/Portion Data", order = 100000000)]
public class PortionData : CountableItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float Value => _value;
    [SerializeField] private float _value;
}