using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
    //현재 필드가 몇번째 필드인지 인식하기 위한 변수
    private int id;
    //최초에 entity를 소환할 위치.
    //entity의 이름을 key값으로 받고, 그 이름을 가진 enemy의 모든 spawnPoints를 value로 가짐.
    private List<string> name;
    private Dictionary<string, Vector2[]> spawnPoints;
    public List<string> Name { get { return name; } } 
    public Dictionary<string, Vector2[]> SpawnPoints { get => spawnPoints; }

    public void InitSettings(int id)
    {
        this.id = id;
        spawnPoints = new Dictionary<string, Vector2[]>();
        name = new List<string>();
    }

    public void ParseData(string name, Vector2[] spawnPoints)
    {
        this.name.Add(name);
        this.spawnPoints[name] = spawnPoints;
        if (SpawnPoints[name] == null)
        {
            Debug.LogError("SpawnPoint 추가에 이상 발생.");
            return;
        }
    }
}
