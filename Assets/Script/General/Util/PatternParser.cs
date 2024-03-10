using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternParser
{
    public EnemyPattern[] EnemyPatternParse(string enemyName) {
        List<EnemyPattern> enemyPattern = new List<EnemyPattern>();
        EnemyPattern enemyPatternTMP = null;
        TextAsset csvData=Resources.Load<TextAsset>("CSV/PatternSheet");

        string[] data = csvData.text.Split(new char[]{ '\n'});

        for (int row = 1; row < data.Length;row++) {
            
            string[] rowData= data[row].Split(new char[] { ',' });
            
            
            if (rowData[1].ToString() == enemyName)
            {
                
                do
                {
                    
                    //Debug.Log("행 : "+data[row]);
                    //Debug.Log("패턴 길이 : " + (rowData[2].Length-1));
                    enemyPatternTMP = new EnemyPattern();
                    enemyPatternTMP.enemyName = enemyName;                         //string타입의 적 이름

                    //enemyPattern.patternLength = System.Int32.Parse(rowData[2]); //int타입의 패턴길이 

                    enemyPatternTMP.patternLength = rowData[2].Length-1;           //int타입의 패턴길이 

                    enemyPatternTMP.patternArray = StringToIntArray(rowData[2]);   //int배열 타입의 패턴 - EX) [4,8,8,4]
                    enemyPattern.Add(enemyPatternTMP);

                    //Debug.Log("////////////////");
                    
                    
                    row++;
                    
                    rowData = data[row].Split(new char[] { ',' });
                }
                while (rowData[0].ToString() == "");

                break;
            }
            
        }
        if (enemyPattern == null) {
            Debug.Log("enemyName을 찾을 수 없습니다.");
        }
        return enemyPattern.ToArray();
    
    }
    private int[] StringToIntArray(string pattern) {
        
        List<int> intList = new List<int>();
        
            for(int i =0;i<pattern.Length-1;i++) {
            
                intList.Add(System.Int32.Parse(pattern[i].ToString()));
                //Debug.Log(pattern[i].ToString());
        
        }
        
        return intList.ToArray();
    
    
    
    }
   
}
