using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class Item_LegatoIncrease : IItem
{
    private int num_n;
    public Item_LegatoIncrease(int num_n, int num_m)
    {
        this.num_n = num_n;
    }

    public float CalcDamage(float originDamage)
    {
        originDamage += originDamage * (num_n / Const.percentageHundred);
        return originDamage;
    }
}
