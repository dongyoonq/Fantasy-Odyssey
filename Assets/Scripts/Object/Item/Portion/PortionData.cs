using UnityEngine;
/// <summary> �Һ� ������ ���� </summary>
[CreateAssetMenu(fileName = "Portion Data", menuName = "Scriptable Object/Portion Data", order = 100000000)]
public class PortionData : CountableItemData
{
    /// <summary> ȿ����(ȸ���� ��) </summary>
    public float Value => _value;
    [SerializeField] private float _value;
}