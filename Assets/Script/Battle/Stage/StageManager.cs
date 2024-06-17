using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using Util.CustomEnum;
using UnityEngine.UI;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Stage stage; // 추후 오프필드에서 특정 적과 조우 시 주입해 줄 예정.
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private GameObject blackBox;
    [SerializeField] private GameObject sceneTransitionPrefab;
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    Metronome metronome;
    private PatternParser parser;
    private JudgeManager judgeManager;
    private IEnemy enemy;
    private EnemyTurn enemyTurn;
    private ActionStartTurn actionStartTurn;
    private ActionEndTurn actionEndTurn;
    private BattleCamManager battleCamManager;

    private GameObject player;
    private PlayerInterface playerInterface;
    GameObject enemyObject;
    GameObject timingUI;
    private BattlePresenter battlePresenter;

    #region battlePos 좌표
    private float battlePosXMin = -15f;
    private float battlePosXMax = 15f;
    private float battlePosYMin = -20f;
    private float battlePosYMax = -7f;
    #endregion

    #region UI
    private DefenseQTE defenseQTE;
    private UIManager UIManager; // 이사 예정입니다~
    private int turnCount = 0;
    private PrepareTurnUI prepareTurnUI;
    private ControlTurnUI turnUI;
    private EnemySignalUI enemySignalUI;
    private Tutorial tutorialUI;
    #endregion

    private PrepareTurn prepareTurn;
    private PlayerTurn playerTurn; //추후 플레이어 턴 완료되면 그 때 변경
    private GuardCounterTurn guardCounterTurn;

    private float secondsPerBeat;
    private bool isStageOver;
    private ITurnHandler[] turnHandler;
    private Turn currentTurn;
    private Vector3 battlePos;

    public event Action OnGameOver;
    public event Action OnStageClear;
    public event Action OnTurnStart;

    #region 프로퍼티
    public float SecondsPerBeat { get { return secondsPerBeat; } }
    public IEnemy Enemy { get { return enemy; } }
    public JudgeManager JudgeManager { get { return judgeManager; } }
    public Turn CurrentTurn { set { currentTurn = value; Debug.Log("여기서 " + currentTurn + " 으로 바뀜"); } }
    public Vector2 BattlePos { get => battlePos; }
    public PlayerInterface PlayerInterface { get => playerInterface; }

    public PrepareTurnUI PrepareTurnUI { get => prepareTurnUI; set { prepareTurnUI = value; } }
    public Tutorial TutorialUI { get => tutorialUI; }
    public BattleCamManager BattleCamManager { get => battleCamManager; }
    public BattlePresenter BattlePresenter { get => battlePresenter; }
    public DefenseQTE DefenseQTE { get => defenseQTE; }
    public int TurnCount { get { IncreaseTurnCount(); return turnCount; } }
    public ControlTurnUI TurnUI { set => turnUI = value; get => turnUI; }
    public EnemySignalUI EnemySignalUI { get => enemySignalUI; set => enemySignalUI = value; }
    public GameObject BlackBox { get => blackBox; }
    public UIManager UIMAnager { get => UIManager; }
  
    #endregion
    #region 디버깅용
    [SerializeField] public Image[] attackIcon;
    [SerializeField] public Image[] defenseIcon;
    #endregion

    [ContextMenu("DEBUG/SceneStart")]
    private void TestPlay()
    {
        InitSettings(stage.BPM, stage.EnemyName, Turn.PrepareTurn, new PlayerInfo(3,7));
        WipeAnimation wipe = Instantiate(sceneTransitionPrefab).transform.GetComponentInChildren<WipeAnimation>();
        wipe.Fade(false);
    }
    [ContextMenu("DEBUG/ResetPlayerPrefs")]
    private void ResetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }


    private void Update()
    {
        if (!isStageOver)
        {
            judgeManager?.UpdateInput();
        }
    }
    public void StageStart(Stage stage , PlayerInfo playerInfo)  //현재는 레벨이랑 경험치를 받고 있지만 확장성을 고려하면 캡슐화시켜서 전달해야 할 듯 합니다.
    {//don't destroy on load에서 주입받을
        this.stage = stage;
        InitSettings(stage.BPM, stage.EnemyName, Turn.PrepareTurn , playerInfo);
        WipeAnimation wipe = Instantiate(sceneTransitionPrefab).transform.GetComponentInChildren<WipeAnimation>();
        wipe.Fade(false);
    }
    private void InitSettings(int bitPerMinute, string enemyName, Turn startTurn,PlayerInfo playerInfo)
    {
        metronome = GetComponent<Metronome>();
        metronome.InitSettins(stage);
        secondsPerBeat = Const.MINUTE_TO_SECOND / bitPerMinute;
        isStageOver = false;
        SetUI();
        SpawnEnemy(enemyName);// 지정된 위치에 소환
        battlePresenter = new GameObject("BattlePresenter").AddComponent<BattlePresenter>();
        battlePresenter.InitSettings(this);
        SpawnPlayer(playerInfo);
        InitObjectsSettings();
        UIManager = new UIManager();
        UIManager.StartStage(this);
        
        LateBindingEvents();
        InitTurnSettings();
        BattleCamSetting();
        StartCoroutine(StageScheduler(startTurn));

        // 아래 두 줄은 상위 매니저에서 호출할 예정입니다~ 아마 게임 매니저일듯?
        // UIManager = new UIManager();
        // UIManager.StartStage(this);

    }

    private IEnumerator StageScheduler(Turn StartTurn)
    {
        currentTurn = StartTurn;
        // 오프닝 연출 추가
        while (!isStageOver)
        {
            ResetSettings();
            yield return turnHandler[(int)currentTurn].TurnStart();
            if (isStageOver)
                break;
            yield return turnHandler[(int)currentTurn].TurnEnd();
        }
        // 게임 종료 이후 연출은 아래 PlayerDie 혹은 EnemyDie에서..
    }

    private void SpawnPlayer(PlayerInfo playerInfo)
    {
        player = Instantiate(Resources.Load<GameObject>("Object/Player"));
        playerInterface = player.GetComponent<PlayerInterface>();
        playerInterface.InitSettings(this);
        playerInterface.PlayerStatus.InitSetting(playerInfo.PlayerLevel,playerInfo.PlayerCurrentEXP);
        playerInterface.PlayerSkillSet.InitSettings(playerInfo.PlayerSkills);
        battlePresenter.InitSettings(this);
        playerInterface.PlayerAttack.InitSettins(this,GetComponent<PlayerAttackTimingCheck>());
    }
    private void SpawnEnemy(string enemyName)
    {
        parser = new PatternParser();
        enemyObject = Instantiate(Resources.Load<GameObject>("Object/Enemy/" + enemyName));
        enemy = enemyObject.GetComponent<Enemy_Prefab>();
        enemy.BindPattern(parser.EnemyPatternParse(enemyName));
        enemyObject.transform.SetParent(enemyTransform);
        enemyObject.transform.SetParent(null);
    }
    private void SetUI() //플레이어,적 스탯과 관련 없는 UI
    {
        GameObject defQTECanvas = Instantiate(Resources.Load<GameObject>("UI/DefenseQTECanvas"));
        defenseQTE = defQTECanvas.GetComponentInChildren<DefenseQTE>(true);
        GameObject tutorialCanvas = Instantiate(Resources.Load<GameObject>(Const.UI_FIRST_TUTORIAL_PATH));
        tutorialUI = tutorialCanvas.GetComponentInChildren<Tutorial>();
        GameObject enemySignalUIAsGameObject = Instantiate(Resources.Load<GameObject>(Const.UI_ENEMYSIGNAL));
        enemySignalUI =enemySignalUIAsGameObject.GetComponentInChildren<EnemySignalUI>();
        enemySignalUI.InitSettings();
        EnemySignalUI = enemySignalUI;
 
    }
    public void OnEnemyDie()
    {
        Debug.Log("스테이지 클리어!");
        OnStageClear?.Invoke();
        isStageOver = true;
    }

    public void OnPlayerDie()
    {
        Debug.Log("게임 오버!");
        OnGameOver?.Invoke();
        isStageOver = true;
    }
    private void InitObjectsSettings()
    {
        #region 디버깅용
        Enemy_Prefab debugEnemy = enemy as Enemy_Prefab;

        #endregion
        judgeManager = new JudgeManager();
        judgeManager.InitSettings(this);
        enemy.InitSettings(this, player.transform);
        player.GetComponent<PlayerTurn>().InitSettings(this, enemyTransform);
        BindEvents();
    }

    private void InitTurnSettings()
    {
        turnHandler = new ITurnHandler[(int)Turn.NumberOfTurnTypes];
        enemyTurn = new EnemyTurn();
        enemyTurn.InitSettings(this);
        turnHandler[(int)Turn.EnemyTurn] = enemyTurn;

        prepareTurn = new PrepareTurn();
        prepareTurn.InitSettings(this);
        turnHandler[(int)Turn.PrepareTurn] = prepareTurn;

        playerTurn = player.GetComponent<PlayerTurn>();
        turnHandler[(int)Turn.PlayerTurn] = playerTurn;

        actionStartTurn = new ActionStartTurn();
        actionStartTurn.InitSettings(this);
        turnHandler[(int)Turn.ActionStartTurn] = actionStartTurn;

        actionEndTurn = new ActionEndTurn();
        actionEndTurn.InitSettings(this);
        turnHandler[(int)Turn.ActionEndTurn] = actionEndTurn;

        guardCounterTurn = new GuardCounterTurn();
        guardCounterTurn.InitSettings(this);
        turnHandler[(int)Turn.GuardCounterTurn] = guardCounterTurn;


    }

    public void SelectRandomBattlePos() {

        battlePos = new Vector3(UnityEngine.Random.Range(battlePosXMin, battlePosXMax), UnityEngine.Random.Range(battlePosYMin,battlePosYMax), -6f);
        battleCamManager.ZoomInTrack = battlePos;

    }

    


    private void BattleCamSetting()
    {

        battleCamManager = FindObjectOfType<BattleCamManager>();
        battleCamManager.InitSetting(this);


    }


    private void BindEvents()
    {
        BindJudgeManagerEvents();
        BindEnemyEvents();
        BindPlayerEvents();
        
    }
    private void BindJudgeManagerEvents()
    {
        judgeManager.OnParryEnd -= enemy.GiveDamage;
        judgeManager.OnParryEnd += enemy.GiveDamage;
        judgeManager.OnComboChange -= enemy.CheckCombo;
        judgeManager.OnComboChange += enemy.CheckCombo;
    }
    private void BindPlayerEvents() {

        
        judgeManager.OnComboChange -= playerInterface.PlayerGuardCounter.CheckCombo;
        judgeManager.OnComboChange += playerInterface.PlayerGuardCounter.CheckCombo;
        playerInterface.PlayerGuardCounter.OnGuardCounterEnd -= judgeManager.ResetCombo;//플레이어 가드카운터의 end이벤트에 구독할 예정
        playerInterface.PlayerGuardCounter.OnGuardCounterEnd += judgeManager.ResetCombo;
        

    }

    private void BindEnemyEvents()
    {
        enemy.OnFramePass -= judgeManager.CheckMissFrame;
        enemy.OnFramePass += judgeManager.CheckMissFrame;
        
        
    }

    private void LateBindingEvents() {
        playerInterface.PlayerTurn.onBasicAttack -= UIManager.BasciAttackQTEControll;
        playerInterface.PlayerTurn.onBasicAttack += UIManager.BasciAttackQTEControll;
        OnGameOver -= enemy.StopActions;
        OnGameOver += enemy.StopActions;
        OnGameOver -= playerInterface.PlayerTurn.StopActions;
        OnGameOver += playerInterface.PlayerTurn.StopActions;
        OnGameOver -= UIManager.InvokeGameOver;
        OnGameOver += UIManager.InvokeGameOver;

        OnStageClear -= enemy.StopActions;
        OnStageClear += enemy.StopActions;
        OnStageClear -= playerInterface.PlayerTurn.StopActions;
        OnStageClear += playerInterface.PlayerTurn.StopActions;
        OnStageClear -= UIManager.InvokeGameClear;
        OnStageClear += UIManager.InvokeGameClear;
    }

    public void IncreaseTurnCount() {
        turnCount++;
    }
    public void ResetSettings()
    {
        OnTurnStart?.Invoke();
    }

    private void OnDestroy()
    {
        if(judgeManager != null)
        {
            judgeManager.OnParryEnd -= enemy.GiveDamage;
            enemy.OnFramePass -= judgeManager.CheckMissFrame;
            OnGameOver -= enemy.StopActions;
            OnStageClear -= enemy.StopActions;
        }
        StopAllCoroutines();
    }
}
