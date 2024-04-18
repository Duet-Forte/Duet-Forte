using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Stage stage; // ���� �����ʵ忡�� Ư�� ���� ���� �� ������ �� ����.
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private GameObject blackBox;

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

    private DefenseQTE defenseQTE;
    private UIManager UIManager; // �̻� �����Դϴ�~
    private int turnCount = 0;
    private PrepareTurnUI prepareTurnUI;
    private ControlTurnUI turnUI;
    private EnemySignalUI enemySignalUI;

    private PrepareTurn prepareTurn;
    private PlayerTurn playerTurn; //���� �÷��̾� �� �Ϸ�Ǹ� �� �� ����
    private GuardCounterTurn guardCounterTurn;

    private float secondsPerBeat;
    private bool isStageOver;
    private ITurnHandler[] turnHandler;
    private CustomEnum.Turn currentTurn;
    private Vector3 battlePos;

    public event Action OnGameOver;
    public event Action OnStageClear;
    public event Action OnTurnStart;

    #region ������Ƽ
    public float SecondsPerBeat { get { return secondsPerBeat; } }
    public IEnemy Enemy { get { return enemy; } }
    public JudgeManager JudgeManager { get { return judgeManager; } }
    public CustomEnum.Turn CurrentTurn { set { currentTurn = value; Debug.Log("���⼭ " + currentTurn + " ���� �ٲ�"); } }
    #endregion
    public Vector2 BattlePos { get => battlePos; }
    public PlayerInterface PlayerInterface { get => playerInterface; }

    public PrepareTurnUI PrepareTurnUI { get => prepareTurnUI; set { prepareTurnUI = value; } }
    public BattleCamManager BattleCamManager { get => battleCamManager; }
    public BattlePresenter BattlePresenter { get => battlePresenter; }
    public DefenseQTE DefenseQTE { get => defenseQTE; }
    public int TurnCount { get { IncreaseTurnCount(); return turnCount; } }
    public ControlTurnUI TurnUI { set => turnUI = value; get => turnUI; }
    public EnemySignalUI EnemySignalUI { get => enemySignalUI; set => enemySignalUI = value; }
    public GameObject BlackBox { get => blackBox; }
    #region ������
    [SerializeField] public Image[] attackIcon;
    [SerializeField] public Image[] defenseIcon;
    #endregion

    private void Awake()
    {
        InitSettings(stage.BPM, stage.EnemyName, CustomEnum.Turn.PrepareTurn);// ���� stage���� ������ �� (bpm, �� �̸�, ���� �� )
    }

    private void Update()
    {
        if (!isStageOver)
        {
            judgeManager?.UpdateInput();
        }
    }

    private void InitSettings(int bitPerMinute, string enemyName, CustomEnum.Turn startTurn)
    {
        secondsPerBeat = Const.MINUTE_TO_SECOND / bitPerMinute;
        isStageOver = false;
        SetUI();
        SpawnEnemy(enemyName);// ������ ��ġ�� ��ȯ
        SpawnPlayer();
        battlePresenter = new GameObject("BattlePresenter").AddComponent<BattlePresenter>();
        battlePresenter.InitSettings(this);
        InitObjectsSettings();
        UIManager = new UIManager();
        UIManager.StartStage(this);
        InitTurnSettings();
        BattleCamSetting();
        StartCoroutine(StageScheduler(startTurn));

        // �Ʒ� �� ���� ���� �Ŵ������� ȣ���� �����Դϴ�~ �Ƹ� ���� �Ŵ����ϵ�?
        // UIManager = new UIManager();
        // UIManager.StartStage(this);

    }

    private IEnumerator StageScheduler(CustomEnum.Turn StartTurn)
    {
        currentTurn = StartTurn;
        // ������ ���� �߰�
        while (!isStageOver)
        {
            ResetSettings();
            yield return turnHandler[(int)currentTurn].TurnStart();
            if (isStageOver)
                break;
            yield return turnHandler[(int)currentTurn].TurnEnd();
        }
        // ���� ���� ���� ������ �Ʒ� PlayerDie Ȥ�� EnemyDie����..
        AkSoundEngine.PostEvent("Combat_Stage_01_BGM", gameObject);
        AkSoundEngine.SetSwitch("Stage01", "StageEnd", gameObject);
    }

    private void SpawnPlayer()
    {
        player = Instantiate(Resources.Load<GameObject>("Object/Player"));
        playerInterface = player.GetComponent<PlayerInterface>();
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
    private void SetUI() //�÷��̾�,�� ���Ȱ� ���� ���� UI
    {
        timingUI = Instantiate(Resources.Load<GameObject>("UI/TimingUI"));
        GameObject defQTECanvas = Instantiate(Resources.Load<GameObject>("UI/DefenseQTECanvas"));
        defenseQTE = defQTECanvas.GetComponentInChildren<DefenseQTE>(true);

        GameObject enemySignalUIAsGameObject = Instantiate(Resources.Load<GameObject>(Const.UI_ENEMYSIGNAL));
        enemySignalUI =enemySignalUIAsGameObject.GetComponentInChildren<EnemySignalUI>();
        enemySignalUI.InitSettings();
        EnemySignalUI = enemySignalUI;

        Debug.Log(defenseQTE);

    }
    public void OnEnemyDie()
    {
        Debug.Log("�������� Ŭ����!");
        OnStageClear?.Invoke();
        isStageOver = true;
    }

    public void OnPlayerDie()
    {
        Debug.Log("���� ����!");
        OnGameOver?.Invoke();
        isStageOver = true;
    }
    private void InitObjectsSettings()
    {
        #region ������
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
        turnHandler = new ITurnHandler[(int)CustomEnum.Turn.NumberOfTurnTypes];
        enemyTurn = new EnemyTurn();
        enemyTurn.InitSettings(this);
        turnHandler[(int)CustomEnum.Turn.EnemyTurn] = enemyTurn;

        prepareTurn = new PrepareTurn();
        #region �߰���ǥ�� prepareTurn

        #endregion
        prepareTurn.InitSettings(this);
        turnHandler[(int)CustomEnum.Turn.PrepareTurn] = prepareTurn;

        playerTurn = player.GetComponent<PlayerTurn>();
        turnHandler[(int)CustomEnum.Turn.PlayerTurn] = playerTurn;

        actionStartTurn = new ActionStartTurn();
        actionStartTurn.InitSettings(this);
        turnHandler[(int)CustomEnum.Turn.ActionStartTurn] = actionStartTurn;

        actionEndTurn = new ActionEndTurn();
        actionEndTurn.InitSettings(this);
        turnHandler[(int)CustomEnum.Turn.ActionEndTurn] = actionEndTurn;

        guardCounterTurn = new GuardCounterTurn();
        guardCounterTurn.InitSettings(this);
        turnHandler[(int)CustomEnum.Turn.GuardCounterTurn] = guardCounterTurn;


    }

    public void SelectRandomBattlePos() {

        battlePos = new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(-4f, -24f), -6f);
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
        OnGameOver -= enemy.StopActions;
        OnGameOver += enemy.StopActions;
        OnStageClear -= enemy.StopActions;
        OnStageClear += enemy.StopActions;
    }
    private void BindJudgeManagerEvents()
    {
        judgeManager.OnMissParry -= enemy.GiveDamage;
        judgeManager.OnMissParry += enemy.GiveDamage;
        judgeManager.OnParrySuccess -= enemy.HandleParryJudge;
        judgeManager.OnParrySuccess += enemy.HandleParryJudge;
        judgeManager.OnComboChange -= enemy.CheckCombo;
        judgeManager.OnComboChange += enemy.CheckCombo;
    }
    private void BindPlayerEvents() {

        
        judgeManager.OnComboChange -= playerInterface.PlayerGuardCounter.CheckCombo;
        judgeManager.OnComboChange += playerInterface.PlayerGuardCounter.CheckCombo;
        
    }

    private void BindEnemyEvents()
    {
        enemy.OnFramePass -= judgeManager.CheckMissFrame;
        enemy.OnFramePass += judgeManager.CheckMissFrame;
        enemy.OnAttack -= judgeManager.DecreaseHealthPoint;
        enemy.OnAttack += judgeManager.DecreaseHealthPoint;
        enemy.OnGuardCounterEnd -= judgeManager.ResetCombo;//�÷��̾� ����ī������ end�̺�Ʈ�� ������ ����
        enemy.OnGuardCounterEnd += judgeManager.ResetCombo;
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
            judgeManager.OnMissParry -= enemy.GiveDamage;
            judgeManager.OnParrySuccess -= enemy.HandleParryJudge;
            enemy.OnFramePass -= judgeManager.CheckMissFrame;
            enemy.OnAttack -= judgeManager.DecreaseHealthPoint;
            OnGameOver -= enemy.StopActions;
            OnStageClear -= enemy.StopActions;
        }
        StopAllCoroutines();
    }
}