using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BICSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject sceneTransitionPrefab;
    [SerializeField] private GameObject loadingImagePrefab;

    private static BICSceneManager instance;
    public static BICSceneManager Instance { get { return instance; } }

    private FieldManager fieldManager;
    public FieldManager FieldManager { get { return fieldManager; } }
    private CameraManager cameraManager;
    public CameraManager CameraManager { get { return cameraManager; } }
    private MusicChanger musicChanger;
    public MusicChanger MusicChanger { get { return musicChanger; } }
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

        cameraManager = new CameraManager();
        musicChanger = new MusicChanger(gameObject);
        fieldManager = new FieldManager();
        cutsceneManager = GetComponent<CutsceneManager>();
        cutsceneManager.InitSettings();
        inputController = GetComponent<InputController>();
        inputController.InitSettings();
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
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Rebuilding SampleStage");
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Top View"));
        fieldManager.Player.GetComponent<PlayerController>().IsStopped = false;
        FieldManager.Field.gameObject.SetActive(true);
        Storage.currentBattleEnemy.Die();
        cameraManager.EnableFieldCamera();
        cutsceneManager.ReplayCutscene();
        MusicChanger.ReplayMusic();
    }
    public void SetFieldScene(PlayerInfo playerInfo = null)
    {
        DataBase.Instance.Player.SetPlayerInfo(playerInfo);
        AkSoundEngine.PostEvent("Combat_BGM_Stop", Metronome.instance.gameObject);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Rebuilding SampleStage");
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Top View"));
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

        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Rebuilding SampleStage"));
        stageManager.StageStart(DataBase.Instance.Stage.GetStage(dataStorage.currentEnemyName) , DataBase.Instance.Player.CreatePlayerInfo());
    }

    private async UniTask LoadBattleScene()
    {
        MusicChanger.StopMusic();
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


