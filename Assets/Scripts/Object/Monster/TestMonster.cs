using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour, IHitable
{
    [SerializeField] int Hp;
    
    public void Hit(int damamge)
    {
        Hp -= damamge;
    }
}
