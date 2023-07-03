using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public int currHp;
    public BaseMonsterData data;
    public MonsterSpawn spawnInfo;

    public abstract void DropItemAndUpdateExp();
}