using System;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance { get { return instance; } }

    private FieldManager fieldManager;
    public FieldManager FieldManager { get { return fieldManager; } }

    private DataStorage dataStorage;
    public DataStorage Storage { get => dataStorage; }
    private CutsceneManager cutsceneManager;
    public CutsceneManager CutsceneManager { get => cutsceneManager; }

    private void Awake()
    {
        InitSetting();
        
    }

    private void InitSetting()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        fieldManager = new FieldManager();
        dataStorage = new DataStorage();
        fieldManager.InitSettings();
        fieldManager.ID = 0;
        cutsceneManager = GetComponent<CutsceneManager>();
        cutsceneManager.InitSettings();
    }
    public void SetBattleScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Rebuilding SampleStage", LoadSceneMode.Additive);
        StageManager stageManager = FindAnyObjectByType<StageManager>();

        if (stageManager != null)
        {
            GameObject go = new GameObject("StageManager");
            stageManager = go.AddComponent<StageManager>();
        }

        //stageManager.InitSettings(dataStorage.Stage);
    }

    private void SetTopViewScene()
    {

    }
}


