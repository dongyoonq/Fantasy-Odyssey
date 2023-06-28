using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Talk Data", menuName = "Scriptable Object/Talk Data", order = 100000)]
public class TalkData : ScriptableObject, ISerializationCallbackReceiver
{
    [NonSerialized] public string id;

    [SerializeField, TextArea] public List<string> talkContents;


    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
    }
}