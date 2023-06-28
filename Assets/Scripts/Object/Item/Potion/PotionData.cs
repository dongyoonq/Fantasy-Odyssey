using UnityEngine;
/// <summary> �Һ� ������ ���� </summary>
[CreateAssetMenu(fileName = "Portion Data", menuName = "Scriptable Object/Portion Data", order = 100000000)]
public class PotionData : CountableItemData
{
    /// <summary> ȿ����(ȸ���� ��) </summary>
    public float Value { get {  return _value; } set { _value = value; } }
    [SerializeField] private float _value;
}