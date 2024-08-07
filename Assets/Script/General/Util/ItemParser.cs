using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParser
{
    
    private TextAsset csvData;
    private string[] data;
    private string[] seperatedData;
    public void ItemParse()
    {
        csvData = Resources.Load<TextAsset>("CSV/ItemTable");
        data = csvData.text.Split('\n');
        
    }

    public IItem GetItem(int itemID)
    {
        for (int row = 0; row < data.Length; row++)
        {
            
            seperatedData=data[row].Split(',');
            //0 : 아이템 타입
            //1 : 아이템 아이디
            //2 : 이펙트 이름
            //3 : 아이템 레벨
            //4 : num_n
            //5 : num_m
            //6 : 효과설명 - 참고용
            //7 : 언락 레벨
            //8 : 가격
            if (seperatedData[1] == itemID.ToString())
            {
                Debug.LogWarning($"{seperatedData[0]},{seperatedData[4]},{seperatedData[5]}");
                if (seperatedData[5] == "")
                {
                    seperatedData[5] = "0";
                }

                return new ItemCreator().Creator(int.Parse(seperatedData[0]), int.Parse(seperatedData[4]), int.Parse(seperatedData[5]));
            }
        }
        Debug.LogError("아이템 ID에 해당하는 아이템을 찾을 수 없습니다.");
        return null;//오류 발생
    }
}
