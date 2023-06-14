using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory
{
    public List<Item> list;
    public int Capacity;

    public Inventory()
    {
        this.list = new List<Item>();
    }
}