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
    private string currentPoint;
    public int ID { get => currentFieldID; set { currentFieldID = value; onFieldIDChange?.Invoke(currentFieldID); } }
    public string Point { get => currentPoint; set { currentPoint = value; onPointChange?.Invoke(currentPoint); } }
    public Field Field { get => currentField; }
    public GameObject Player { get {return player;} }
    public event Action<int> onFieldIDChange;
    public event Action<string> onPointChange;

    //초기화와 동시에 FieldManager의 참조를 필요로 해서 함수로 변경
    public void InitSettings(int saveID)
    {

        fieldPrefabs = new Dictionary<int, GameObject>();
        BindEvent();
        if (saveID == -1)
        {
            ID = 0;
        }
        else
        {
            ID = DataBase.Field.ID;
            SpawnPlayer(saveID);
            Point = currentField.GetSavePoint(ID).pointName;
            GameManager.FieldManager.Field.GetEntity("Timmy").InitSettings("Timmy", GameManager.FieldManager.Field.GetCutsceneObject("Timmy").transform.position);
            GameManager.CameraManager.SetFollowCamera();
            currentField.DisableCutsceneObjects();
            Debug.Log(Point);
        }
    }
    public void SpawnPlayer(Vector2 spawnPoint)
    {
        if(player == null)
        {
            GameObject playerPrefab = Resources.Load<GameObject>(Util.Const.TOPVIEW_PLAYER);
            player = UnityEngine.Object.Instantiate(playerPrefab);
        }
        player.transform.position = spawnPoint;
        player.transform.SetParent(currentField.transform);
    }

    public void SpawnPlayer(int saveID)
    {
        SavePoint data = currentField.GetSavePoint(saveID);
        Point = data.pointName;
        SpawnPlayer(data.spawnPoint);
    }
    public void SpawnEntity(string entityName, Vector2 spawnPoint)
    {
        TopViewEntity temp = currentField.GetEntity(entityName);
        temp.gameObject.SetActive(true);
        temp.transform.position = spawnPoint;
        temp.transform.SetParent(currentField.transform);
    }
    private void BindEvent()
    {
        onFieldIDChange -= DataBase.Field.ChangeField;
        onFieldIDChange += DataBase.Field.ChangeField;
        onFieldIDChange -= SetField;
        onFieldIDChange += SetField;
        onPointChange -= GameManager.CameraManager.ChangeCameraCollider;
        onPointChange += GameManager.CameraManager.ChangeCameraCollider;
        onPointChange -= GameManager.MusicChanger.SetMusic;
        onPointChange += GameManager.MusicChanger.SetMusic;
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

    public void CheckPoint()
    {
        if (player == null)
            return;
        Vector2 origin = player.transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector3.forward, Mathf.Infinity);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Point"))
            {
                Point = hit.collider.name;
                return;
            }
        }
    }
}
