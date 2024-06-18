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
    public FieldManager()
    {
        fieldPrefabs = new Dictionary<int, GameObject>();
        BindEvent();
        DataBase.Instance.Dialogue.ResetLine(0);
        SetField(0);
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
    public void SpawnEntity(string entityName, Vector2 spawnPoint)
    {
        TopViewEntity temp = currentField.GetEntity(entityName);
        temp.gameObject.SetActive(true);
        temp.transform.position = spawnPoint;
        temp.transform.SetParent(currentField.transform);
    }
    private void BindEvent()
    {
        onFieldIDChange -= DataBase.Instance.Dialogue.ResetLine;
        onFieldIDChange += DataBase.Instance.Dialogue.ResetLine;
        onFieldIDChange -= SetField;
        onFieldIDChange += SetField;
        onPointChange -= SceneManager.Instance.CameraManager.ChangeCameraCollider;
        onPointChange += SceneManager.Instance.CameraManager.ChangeCameraCollider;
        onPointChange -= SceneManager.Instance.MusicChanger.SetMusic;
        onPointChange += SceneManager.Instance.MusicChanger.SetMusic;
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
