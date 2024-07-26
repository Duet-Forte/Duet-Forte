using System.Collections.Generic;
using UnityEngine;

public class FieldDataBase
{
    private int fieldID;
    public int ID { get => fieldID; }
    private Dictionary<string, GameObject> prefabs;
    public Dictionary<string, GameObject> Prefabs
    {
        get
        {
            if (prefabs == null)
            {
                GameObject[] entityPrefabsArray = Resources.LoadAll<GameObject>(Util.Const.TOPVIEW_ENTITY);
                prefabs = new Dictionary<string, GameObject>();
                foreach (var go in entityPrefabsArray)
                {
                    prefabs.Add(go.name, go);
                }
            }
            return prefabs;
        }
    }

    public void ChangeField(int fieldID)
    {
        this.fieldID = fieldID;
        DataBase.Dialogue.ResetLine(fieldID);
    }
}
