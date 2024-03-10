using System;
using UnityEngine;

public abstract class TopViewObject : MonoBehaviour
{
    protected string objectName;
    protected int id;
    public string Name { get { return objectName; } }
    public int Id { get { return id; } }

    public virtual void InitSettings(string name, int id = 0)
    {
        objectName = name;
        this.id = id;
    }
}
