using System;
using System.Collections.Specialized;
using UnityEngine;

public abstract class TopViewEntity : MonoBehaviour
{
    protected string objectName;
    protected int id;
    public string Name { get { return objectName; } }
    public int Id { get { return id; } }

    public virtual void InitSettings(string name, Vector2 initialSpawnPoint, int id = 0)
    {
        objectName = name;
        this.id = id;
        transform.position = initialSpawnPoint;
    }
}
