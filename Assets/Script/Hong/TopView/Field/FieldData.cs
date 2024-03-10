using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
    //���� �ʵ尡 ���° �ʵ����� �ν��ϱ� ���� ����
    private int id;
    //���ʿ� entity�� ��ȯ�� ��ġ.
    //entity�� �̸��� key������ �ް�, �� �̸��� ���� enemy�� ��� spawnPoints�� value�� ����.
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
            Debug.LogError("SpawnPoint �߰��� �̻� �߻�.");
            return;
        }
    }
}
