using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AgressiveMonster Data", menuName = "Scriptable Object/AgressiveMonster Data", order = 1)]
public class AgressiveMonsterData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("�������� ���� ����")]
    [SerializeField] float _detectRange;

    //public float detectRange { get { return _detectRange; } }

    [NonSerialized] public float DetectRange;

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        DetectRange = _detectRange;
    }
}