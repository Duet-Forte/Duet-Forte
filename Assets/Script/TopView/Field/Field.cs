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
    private Dictionary<string, TopViewEntity[]> entities;
    private Dictionary<string, GameObject> cutsceneObject;
    private Dictionary<string, CinemachinePath> cameraPaths;
    
    public void InitSettings(int id)
    {
        this.id = id;
        entities = new Dictionary<string, TopViewEntity[]>();
        cutsceneObject = new Dictionary<string, GameObject>();
        SetEntity();
        SetCutsceneObject();
        SetCameraPaths();
    }
    public void SetEntity()
    {
        StringBuilder sb = new StringBuilder();

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
                GameObject enemy = Instantiate(enemyPrefab);
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

    private void SetCameraPaths()
    {
        cameraPaths = new Dictionary<string, CinemachinePath>();
        for(int count = 0; count < cameraPathsParent.childCount; ++count)
        {
            CinemachinePath pathTransform = cameraPathsParent.GetChild(count).GetComponent<CinemachinePath>();
            cameraPaths.Add(pathTransform.name, pathTransform);
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
        entities.TryGetValue(name, out var entity);
        return entity[id];
    }

    public GameObject GetCutsceneObject(string name)
    {
        cutsceneObject.TryGetValue(name, out var entity);
        return entity;
    }

    public CinemachinePath SetCameraPath(CinemachineVirtualCamera virtualCamera, string cutsceneName)
    {
        cameraPaths.TryGetValue(cutsceneName, out CinemachinePath cutscenePath);
        virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = cutscenePath;
        return cutscenePath;
    }
}