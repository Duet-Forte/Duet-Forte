using System.Collections.Generic;
using UnityEngine;

public class Field
{
    private Dictionary<string, TopViewObject[]> entities;
    private FieldData data;
    public Field(int id)
    {
        InitSetting(id);
    }
    public TopViewObject Entity(string name, int id = 0)
    {
        if(!entities.TryGetValue(name, out var interactable))
            return null;
        return interactable[id];
    }

    public void InitSetting(int id)
    {
        data = new FieldData();
        data.InitSettings(id);
        entities = new Dictionary<string, TopViewObject[]>();
    }

    public void ParseData(string name, Vector2[] spawnPoints)
    {
        data.ParseData(name, spawnPoints);      
    }

    public void InitAllEntity()
    {
        foreach(var name in data.Name)
        {
            ParseEntityGroup(name);
        }
    }

    public void ParseEntityGroup(string name)
    {
        GameObject prefab = DataBase.Instance.Entity.Prefabs[name];
        int spawnCount = data.SpawnPoints[name].Length;
        TopViewObject[] topViewObjects = new TopViewObject[spawnCount];

        for (int count = 0; count < spawnCount; ++count)
        {
            GameObject entityObject = Object.Instantiate(prefab);
            TopViewObject topViewEntity = entityObject.GetComponent<TopViewObject>();
            if (topViewEntity == null) 
            {
                Debug.Log($"{name} 객체에 컴포넌트가 없습니다.");
                return;
            }
            entityObject.SetActive(false);

            topViewEntity.InitSettings(name, count);
            topViewObjects[count] = topViewEntity;
        }
        
        entities.Add(name, topViewObjects);
    }

    public void SpawnEntity(TopViewObject entity, Vector2 spawnPoint)
    {
        entity.gameObject.SetActive(true);
        entity.transform.position = spawnPoint;
    }

    public void SpawnEntity(TopViewObject entity)
    {
        entity.gameObject.SetActive(true);
        entity.transform.position = data.SpawnPoints[entity.Name][entity.Id];
    }

    public void SpawnAllEntity()
    {
        foreach(var dataPair in entities)
        {
            foreach (var entity in dataPair.Value)
                entity.gameObject.SetActive(true);
        }
    }
}
