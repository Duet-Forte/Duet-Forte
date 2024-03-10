using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FieldManager
{
    private Field[] fields;
    private GameObject playerPrefab;
    private GameObject player;
    private int currentFieldId;
    public int FieldID { get { return currentFieldId; } set { currentFieldId = value; onFieldIDChange?.Invoke(currentFieldId); } }

    public GameObject Player { get {return player;} }
    public event Action<int> onFieldIDChange;

    public void InitSettings()
    {
        List<Dictionary<string, string>> fieldData = TSVReader.Read("FieldData.tsv");
        playerPrefab = Resources.Load<GameObject>("TopView/Player");
        int numberOfFields = TSVReader.FindRepeatNumber(fieldData , "ID");
        fields = new Field[numberOfFields];
        TSVReader.ParseData(fieldData, "ID", ParseEntity);
        BindEvent();
    }

    public void SpawnPlayer(Vector2 spawnPoint)
    {
        if(player == null)
            player = UnityEngine.Object.Instantiate(playerPrefab);
        player.transform.position = spawnPoint;
    }

    public void SpawnEntity(string name, int id = 0)
    {
        TopViewObject topViewObject = fields[currentFieldId].Entity(name, id);
        if (topViewObject != null)
            fields[currentFieldId].SpawnEntity(topViewObject);
        else
            Debug.Log($"스폰 실패! {name}의 오브젝트가 존재하지 않습니다!");
    }
    public void SpawnEntityAtPoint(string name, Vector2 spawnPoint, int id = 0)
    {
        TopViewObject topViewObject = fields[currentFieldId].Entity(name, id);
        if (topViewObject != null)
            fields[currentFieldId].SpawnEntity(topViewObject, spawnPoint);
        else
            Debug.Log($"스폰 실패! {name}의 오브젝트가 존재하지 않습니다!");
    }
    public void SpawnEntityGroup(string name)
    {
        fields[currentFieldId].ParseEntityGroup(name);
    }
    public void EnterBattle(string name)
    {

    }


    private void ParseEntity(List<Dictionary<string, string>> fieldData, int[] startColumn, int id)
    {
        fields[id] = new Field(id);
        StringBuilder name = new StringBuilder();
        List<Vector2> spawnPoints = new List<Vector2>();

        for (int column = startColumn[id]; column < startColumn[id + 1]; ++column)
        {
            if (fieldData[column]["Name"] != string.Empty)
            {
                if (spawnPoints.Count > 0)
                {
                    fields[ id].ParseData(name.ToString(), spawnPoints.ToArray());
                    spawnPoints.Clear();
                    name.Clear();
                }
                name.Append(fieldData[column]["Name"]);
            }

            int spawnPointX = int.Parse(fieldData[column]["Spawn X"]);
            int spawnPointY = int.Parse(fieldData[column]["Spawn Y"]);
            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);
            spawnPoints.Add(spawnPoint);
        }

        fields[id].ParseData(name.ToString(), spawnPoints.ToArray());
        fields[id].InitAllEntity();
        spawnPoints.Clear();
        name.Clear();
    }

    private void BindEvent()
    {
        onFieldIDChange -= DataBase.Instance.Dialogue.ResetLine;
        onFieldIDChange += DataBase.Instance.Dialogue.ResetLine;
    }
}
