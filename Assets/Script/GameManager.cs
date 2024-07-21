using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject sceneTransitionPrefab;
    [SerializeField] private GameObject loadingImagePrefab;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private FieldManager fieldManager;
    public static FieldManager FieldManager { get { return instance.fieldManager; } }
    private CameraManager cameraManager;
    public static CameraManager CameraManager { get { return instance.cameraManager; } }
    private  MusicChanger musicChanger;
    public static MusicChanger MusicChanger { get { return instance.musicChanger; } }
    private DataStorage dataStorage;
    public static DataStorage Storage { get => instance.dataStorage; }
    private CutsceneManager cutsceneManager;
    public static CutsceneManager CutsceneManager { get => instance.cutsceneManager; }

    private InputController inputController;
    public static InputController InputController { get {  return instance.inputController; } }

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

        cameraManager = new CameraManager();
        musicChanger = new MusicChanger(gameObject);
        int saveInt = DataBase.Instance.Load();
        fieldManager = new FieldManager();
        fieldManager.InitSettings(saveInt);
        inputController = GetComponent<InputController>();
        inputController.InitSettings();
        cutsceneManager = GetComponent<CutsceneManager>();
        cutsceneManager.InitSettings(saveInt);
        dataStorage = new DataStorage();
    }

    public void SetBattleScene(string name)
    {
        if(fieldManager.Player != null)
            fieldManager.Player.GetComponent<PlayerController>().Stop();
        dataStorage.currentEnemyName = name;
        MusicChanger.StopMusic();        
        WipeAnimation wipe = Instantiate(sceneTransitionPrefab).transform.GetComponentInChildren<WipeAnimation>();
        wipe.Fade(true, null, InitBattleScene);
    }
    [ContextMenu("³¡")]
    public void SetFieldScene()
    {
        AkSoundEngine.PostEvent("Combat_BGM_Stop", Metronome.instance.gameObject);
        SceneManager.UnloadSceneAsync("Rebuilding SampleStage");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Top View"));
        fieldManager.Player.GetComponent<PlayerController>().IsStopped = false;
        FieldManager.Field.gameObject.SetActive(true);
        Storage.currentBattleEnemy.Die();
        cameraManager.EnableFieldCamera();
        cutsceneManager.ReplayCutscene();
        MusicChanger.ReplayMusic();
    }
    public void SetFieldScene(PlayerInfo playerInfo = null)
    {
        DataBase.Player.SetPlayerInfo(playerInfo);
        AkSoundEngine.PostEvent("Combat_BGM_Stop", Metronome.instance.gameObject);
        SceneManager.UnloadSceneAsync("Rebuilding SampleStage");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Top View"));
        FieldManager.Field.gameObject.SetActive(true);
        if(Storage.currentBattleEnemy != null)
        {
            Storage.currentBattleEnemy.Die();
            Storage.currentBattleEnemy = null;
        }
        if(fieldManager.Player != null)
            fieldManager.Player.GetComponent<PlayerController>().IsStopped = false;
        MusicChanger.ReplayMusic();
        cameraManager.EnableFieldCamera();
        cutsceneManager.ReplayCutscene();
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

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Rebuilding SampleStage"));
        stageManager.StageStart(DataBase.Stage.GetStage(dataStorage.currentEnemyName) , DataBase.Player.CreatePlayerInfo());
    }

    private async UniTask LoadBattleScene()
    {
        MusicChanger.StopMusic();
        AsyncOperation operation = SceneManager.LoadSceneAsync("Rebuilding SampleStage", LoadSceneMode.Additive);
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


