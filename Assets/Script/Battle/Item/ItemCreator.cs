using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator
{
    public IItem Creator(int itemType, int num_n,int num_m)
    {
        switch (itemType)
        {
            case 101:
                return new Item_StaccatoIncrease(num_n, num_m); 
            case 102:
                return new Item_LegatoIncrease(num_n, num_m);
            case 105:  break;
        }
        Debug.LogError("정의되지 않은 아이템 타입입니다.");
        return null; // 오류 발생!
    }
}
