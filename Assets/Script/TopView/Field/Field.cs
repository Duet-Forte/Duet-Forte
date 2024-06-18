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
    [SerializeField] private CinemachineVirtualCamera cutsceneCamera;
    private Dictionary<string, TopViewEntity[]> entities;
    private Dictionary<string, GameObject> cutsceneObject;
    private Dictionary<string, CinemachinePath> cameraPaths;
    private Dictionary<string, PolygonCollider2D> cameraColliders;
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
            // temp는 특정 에너미의 스폰포인트의 부모입니다.
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

                // NPC의 경우, 모든 스폰 포인트에 생성한 후, InitSettings에서 현재 ID의 객체를 제외하고는 전부 Destroy
                // 몬스터의 경우, 생존 여부를 파악 후, InitSettings에서 생성 여부 결정
                tempEnemies[tempChildID].InitSettings(enemyName, enemySpawnPoint, tempChildID);
            }

            entities.Add(enemyName, tempEnemies);
            sb.Clear();
        }
    }

    private void SetCameraSettings()
    {
        BICSceneManager.Instance.CameraManager.SetCutsceneCamera(cutsceneCamera);
        cameraPaths = new Dictionary<string, CinemachinePath>();
        cameraColliders = new Dictionary<string, PolygonCollider2D>();

        for(int count = 0; count < cameraPathsParent.childCount; ++count)
        {
            CinemachinePath pathTransform = cameraPathsParent.GetChild(count).GetComponent<CinemachinePath>();
            cameraPaths.Add(pathTransform.name, pathTransform);
        }

        for (int count = 0; count < cameraColliderParent.childCount; ++count)
        {
            PolygonCollider2D colliderTransform = cameraColliderParent.GetChild(count).GetComponent<PolygonCollider2D>();
            cameraColliders.Add(colliderTransform.name, colliderTransform);
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
        cameraColliders.TryGetValue(regionName, out PolygonCollider2D collider);
        return collider;
    }

    public void DisableCutsceneObjects()
    {
        cutsceneObjectParent.gameObject.SetActive(false);
    }
}