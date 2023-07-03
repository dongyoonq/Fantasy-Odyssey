using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum State
    {
        Empty,
        Use,
    }

    [NonSerialized] public State state;
}