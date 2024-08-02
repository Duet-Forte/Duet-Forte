using Cinemachine;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Util;

public class Field : MonoBehaviour
{
    private int id;
    [SerializeField] private Transform entitySpawnParent;
    [SerializeField] private Transform cutsceneObjectParent;
    [SerializeField] private Transform cameraPathsParent;
    [SerializeField] private Transform cameraColliderParent;
    [SerializeField] private Transform savePointParent;
    [SerializeField] private CinemachineVirtualCamera cutsceneCamera;
    private Dictionary<string, TopViewEntity[]> entities;
    private Dictionary<string, GameObject> cutsceneObject;
    private Dictionary<string, CinemachinePath> cameraPaths;
    public Dictionary<string, Point> points;
    private StringBuilder sb;

    public CinemachineVirtualCamera CutsceneCam { get => cutsceneCamera; }
    public void InitSettings(int id)
    {
        this.id = id;
        sb = new StringBuilder();
        entities = new Dictionary<string, TopViewEntity[]>();
        cutsceneObject = new Dictionary<string, GameObject>();
        SetEntity();
        SetCutsceneObject();
        SetCameraSettings();
    }
    public void SetEntity()
    {
        for (int count = 0; count < entitySpawnParent.childCount; ++count)
        {
            // temp�� Ư�� ���ʹ��� ��������Ʈ�� �θ��Դϴ�.
            Transform temp = entitySpawnParent.GetChild(count);

            string enemyName = temp.name;
            sb.Append(Const.TOPVIEW_ENTITY);
            sb.Append(enemyName);
            GameObject enemyPrefab = Resources.Load<GameObject>(sb.ToString());

            TopViewEntity[] tempEnemies = new TopViewEntity[temp.childCount];
            for(int tempChildID = 0; tempChildID < temp.childCount; ++tempChildID)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform);
                enemy.name = enemyName;
                tempEnemies[tempChildID] = enemy.GetComponent<TopViewEntity>();
                Vector2 enemySpawnPoint = temp.GetChild(tempChildID).position;

                // NPC�� ���, ��� ���� ����Ʈ�� ������ ��, InitSettings���� ���� ID�� ��ü�� �����ϰ��� ���� Destroy
                // ������ ���, ���� ���θ� �ľ� ��, InitSettings���� ���� ���� ����
                tempEnemies[tempChildID].InitSettings(enemyName, enemySpawnPoint, tempChildID);
            }

            entities.Add(enemyName, tempEnemies);
            sb.Clear();
        }
    }

    private void SetCameraSettings()
    {
        GameManager.CameraManager.SetCutsceneCamera(cutsceneCamera);
        cameraPaths = new Dictionary<string, CinemachinePath>();
        points = new Dictionary<string, Point>();

        for(int count = 0; count < cameraPathsParent.childCount; ++count)
        {
            CinemachinePath pathTransform = cameraPathsParent.GetChild(count).GetComponent<CinemachinePath>();
            cameraPaths.Add(pathTransform.name, pathTransform);
        }
        for (int count = 0; count < cameraColliderParent.childCount; ++count)
        {
            Point point = cameraColliderParent.GetChild(count).GetComponent<Point>();
            points.Add(point.name, point);
        }
    }
    public void SetCutsceneObject()
    {
        for(int count = 0; count < cutsceneObjectParent.childCount; ++count)
        {
            GameObject tempCutsceneObject = cutsceneObjectParent.GetChild(count).gameObject;
            cutsceneObject.Add(tempCutsceneObject.name, tempCutsceneObject);
        }
    }

    public TopViewEntity GetEntity(string name, int id = 0)
    {
        if (entities.TryGetValue(name, out var entity))
        {
            return entity[id];
        }
        else
        {
            sb.Clear();
            sb.Append("TopView/Entity/");
            sb.Append(name);
            TopViewEntity[] temp = new TopViewEntity[1];
            temp[0] = Instantiate(Resources.Load<GameObject>(sb.ToString()), transform).GetComponent<TopViewEntity>();
            entities.Add(name, temp);
            return temp[0];
        }
    }

    public GameObject GetCutsceneObject(string name)
    {
        cutsceneObject.TryGetValue(name, out var entity);
        return entity;
    }

    public CinemachinePath SetCameraPath(string cutsceneName)
    {
        cameraPaths.TryGetValue(cutsceneName, out CinemachinePath cutscenePath);
        cutsceneCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = cutscenePath;
        return cutscenePath;
    }

    public PolygonCollider2D GetCameraCollider(string regionName)
    {
        points.TryGetValue(regionName, out Point point);
        return point.GetComponent<PolygonCollider2D>();
    }

    public void DisableCutsceneObjects()
    {
        cutsceneObjectParent.gameObject.SetActive(false);
    }

    public SavePoint GetSavePoint(int saveInt)
    {
        return savePointParent.GetChild(saveInt).GetComponent<SavePoint>();
    }
}