using UnityEngine;
/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Portion Data", menuName = "Scriptable Object/Portion Data", order = 100000000)]
public class PotionData : CountableItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float Value { get {  return _value; } set { _value = value; } }
    [SerializeField] private float _value;
}