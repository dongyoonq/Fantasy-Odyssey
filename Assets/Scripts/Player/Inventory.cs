using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory
{
    public List<Item> list;
    public int Capacity = 32;
    public int currCount;

    public Inventory()
    {
        this.list = new List<Item>();
    }
}