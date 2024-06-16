using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Util;

public class TSVReader
{
    public static List<Dictionary<string, string>> Read(string file)
    {
        List<Dictionary<string, string>> text = new List<Dictionary<string, string>>();
        StreamReader streamReader = new StreamReader(Application.dataPath + "/Resources/TSV/" + file);
        string[] header = streamReader.ReadLine().Split("\t");
        // 0번째 행은 헤더이기 때문에 null로 설정.
        text.Add(null);
        int column = 1;

        while (!streamReader.EndOfStream)
        {
            // 한 줄씩 읽어오기
            string[] line = streamReader.ReadLine().Split("\t");
            if (line[4] == string.Empty && line[5] == string.Empty)
                continue;
            text.Add(new Dictionary<string, string>());
            for (int index = 0; index < header.Length; ++index)
            {
                text[column][header[index]] = line[index];
            }

            column++;
        }

        streamReader.Close();
        return text;
    }

    public static int FindRepeatNumber(List<Dictionary<string, string>> data, string header)
    {
        int repeat = 0;
        for (int column = 1; column < data.Count; column++)
        {
            if (data[column][header] != string.Empty)
            {
                ++repeat;
            }
        }
        return repeat;
    }

    /// <summary>
    ///  헤더를 기준으로 데이터 파싱하는 함수
    /// </summary>
    /// <param name="data">파싱한 데이터 딕셔너리</param>
    /// <param name="header">기준점이 되는 헤더</param>
    /// <param name="onCategoryChange">헤더가 바뀔 때마다 호출되어야 하는 함수</param>
    public static void ParseData(List<Dictionary<string, string>> data, string header, Action<List<Dictionary<string, string>>, int[], int> onCategoryChange)
    {
        int numberOfCategory = FindRepeatNumber(data, header);
        int[] startColumns = new int[numberOfCategory + 1]; //마지막 줄을 알 수 있게 필드의 수보다 하나를 더 추가.
        int column = 1;
        int maxColumn = data.Count;

        for (int id = 0; id < numberOfCategory; ++id)
        {
            startColumns[id] = column;

            do
            {
                // 최대 행은 length이기에, index로 맞추기 위해 -1을 함.
                if (column >= maxColumn - 1)
                    break;
                column++;
            }
            while (data[column][header] == string.Empty);
        }

        // 각 필드의 시작 행을 기준으로 에너미를 파싱하기 때문에, 가장 마지막에는 더미 필드의 행을 추가해 그 사이의 값을 파싱함.
        startColumns[numberOfCategory] = maxColumn;

        for (int id = 0; id < numberOfCategory; ++id)
        {
            onCategoryChange(data, startColumns, id);
        }
    }
}
