using System;
using UnityEngine;

public class SpiderSpawnPoint : MonoBehaviour
{
    public enum State
    {
        Empty,
        Use,
    }

    [NonSerialized] public State state;
}