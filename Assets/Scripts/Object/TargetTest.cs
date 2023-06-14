using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour, IHitable
{
    public int Hp = 100;

    public void Hit(int damamge)
    {
        Hp -= damamge;

        if (Hp <= 0)
            Destroy(gameObject);
    }
}
