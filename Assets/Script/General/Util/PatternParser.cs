using System;
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
            
            string[] rowData= data[row].Split(",");
            if (rowData[1].ToString() == enemyName)
            {
                
                do
                {
                    enemyPatternTMP = new EnemyPattern();
                    enemyPatternTMP.enemyName = enemyName;                         //stringŸ���� �� �̸�
                    enemyPatternTMP.patternLength = rowData[2].Length-1;           //intŸ���� ���ϱ��� 
                    string[] tmp=rowData[2].Split('.');
                    enemyPatternTMP.patternArray = Array.ConvertAll(tmp, s => int.Parse(s));
                    enemyPattern.Add(enemyPatternTMP);
                    row++;
                    
                    rowData = data[row].Split(new char[] { ',' });
                }
                while (rowData[0].ToString() == "");

                break;
            }
            
        }
        if (enemyPattern == null) {
            Debug.Log("enemyName�� ã�� �� �����ϴ�.");
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
