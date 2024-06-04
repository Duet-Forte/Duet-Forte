using Cysharp.Threading.Tasks;
using System;
using System.Data;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [Header ("Debug")]
    [SerializeField] private Stage testStage;
    [SerializeField] private GameObject sceneTransitionPrefab;
    [SerializeField] private GameObject loadingImagePrefab;

    private static SceneManager instance;
    public static SceneManager Instance { get { return instance; } }

    private FieldManager fieldManager;
    public FieldManager FieldManager { get { return fieldManager; } }
    private CameraManager cameraManager;
    public CameraManager CameraManager { get { return cameraManager; } }
    private DataStorage dataStorage;
    public DataStorage Storage { get => dataStorage; }
    private CutsceneManager cutsceneManager;
    public CutsceneManager CutsceneManager { get => cutsceneManager; }

    private InputController inputController;
    public InputController InputController { get {  return inputController; } }

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
        cameraManager = new CameraManager();
        fieldManager.InitSettings();
        fieldManager.ID = 0;
        cameraManager.InitSetting();
        cutsceneManager = GetComponent<CutsceneManager>();
        cutsceneManager.InitSettings();
        inputController = GetComponent<InputController>();
        inputController.InitSettings();
    }

    public void SetBattleScene(string name)
    {
        WipeAnimation wipe = Instantiate(sceneTransitionPrefab).transform.GetComponentInChildren<WipeAnimation>();
        wipe.Fade(true, null, InitBattleScene);
    }

    public void SetTopViewScene()
    {
        FieldManager.Field.gameObject.SetActive(true);
    }

    private async void InitBattleScene()
    {
        await LoadBattleScene();
        Camera.main.gameObject.SetActive(false);
        FieldManager.Field.gameObject.SetActive(false);
        StageManager stageManager = FindAnyObjectByType<StageManager>();

        if (stageManager == null)
        {
            GameObject go = new GameObject("StageManager");
            stageManager = go.AddComponent<StageManager>();
            go.AddComponent<Metronome>();
        }

        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Rebuilding SampleStage"));
        stageManager.StageStart(testStage,null);
    }

    private async UniTask LoadBattleScene()
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Rebuilding SampleStage", LoadSceneMode.Additive);
        LoadingImage loadingImage = Instantiate(loadingImagePrefab).GetComponent<LoadingImage>();
        loadingImage.InitSettings();

        while(!operation.isDone)
        {
            float percentage = Mathf.Clamp01(operation.progress / 0.9f);
            loadingImage.SetFillAmount(percentage);
            await UniTask.DelayFrame(1);
        }

        Destroy(loadingImage.gameObject);
    }
}


