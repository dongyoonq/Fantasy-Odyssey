using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState<TOwner> where TOwner : MonoBehaviour
{
    protected TOwner owner;

    public MonsterBaseState(TOwner owner)
    {
        this.owner = owner;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}