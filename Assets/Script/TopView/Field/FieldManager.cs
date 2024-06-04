using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FieldManager
{
    private int currentFieldID;
    private GameObject player;
    private Dictionary<int, GameObject> fieldPrefabs;
    private Field currentField;
    public int ID { get => currentFieldID; set { currentFieldID = value; onFieldIDChange?.Invoke(currentFieldID); } }
    public Field Field { get => currentField; }
    public GameObject Player { get {return player;} }
    public event Action<int> onFieldIDChange;

    public FieldManager()
    {
        fieldPrefabs = new Dictionary<int, GameObject>();
        BindEvent();
        ID = 0;
    }
    public void SpawnPlayer(Vector2 spawnPoint)
    {
        if(player == null)
        {
            GameObject playerPrefab = Resources.Load<GameObject>(Util.Const.TOPVIEW_PLAYER);
            player = UnityEngine.Object.Instantiate(playerPrefab);
        }
        player.transform.position = spawnPoint;
    }

    private void BindEvent()
    {
        onFieldIDChange -= DataBase.Instance.Dialogue.ResetLine;
        onFieldIDChange += DataBase.Instance.Dialogue.ResetLine;
        onFieldIDChange -= SetField;
        onFieldIDChange += SetField;
    }

    private void SetField(int fieldIndex)
    {
        if (fieldPrefabs.Count == 0 || fieldPrefabs[fieldIndex] == null)
        {
            // Test Code
            fieldPrefabs.Add(fieldIndex, Resources.Load<GameObject>("TopView/0"));
        }
        if(currentField != null)
            GameObject.Destroy(currentField.gameObject);
        GameObject field = GameObject.Instantiate(fieldPrefabs[fieldIndex]);
        currentField = field.GetComponent<Field>();
        currentField.InitSettings(fieldIndex);
    }
}
