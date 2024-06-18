using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.CustomEnum;
using UnityEngine.UI;
using SoundSet;
using Util.CustomEnum;
using Director;
using UnityEngine.Rendering;
public class Enemy_Prefab : MonoBehaviour, IEnemy
{
    private StageManager stageManager;
    private EnemyData data;
    private EnemyPattern[] enemyPattern;
    private EnemyAnimator enemyAnimator;


    #region 기본적인 스테이터스
    [Header("Enemy Info")]
    [Space(5f)]
    [Tooltip("기능적 이름, hitparticle의 컨벤션과 이름이 같아야 한다.")]
    [SerializeField] string enemyName;
    [SerializeField] string flavorTextName;
    [TextArea]
    [SerializeField] string enemyInfo;
    [SerializeField] Sprite enemyImage;
    [Header("Entity Stats")]
    [Space(5f)]
    [SerializeField] private int healthPoint;
    [SerializeField] private float enemyAttack;
    [SerializeField] private int exp;
    /// <summary>
    /// 방어력은 빼기공식을 사용
    /// slash 피해 순수 대미지= slashAttack - slashDefense (순수 대미지는 항상 0보다 크거나 같음)
    /// </summary>
    [SerializeField] private float slashDefense;
    [SerializeField] private float pierceDefense;
    [Header("Sounds")]
    [Space(5f)]
    [SerializeField] private string enemyAttackSoundEvent;
    [SerializeField] private SignalInstrument signalInstrument; //시그널 사운드 고르기
    [Space(10f)]
    #endregion
    
    
    private float attackDelay;
    private double timeOffset;
    private int currentNoteIndex;

    private List<double> targetTimes;
    private List<bool> isNoteChecked; //공격 하나당 판정 여부 리스트

    private double judgeStartTime;
    private double judgeEndTime;
    private int patternLength;
    private int[] patternArray;
    private bool isEnteringGuardCounterPhase;
    private bool isSignalEnd=false;
    

    public event Action OnFramePass;
    public event Action OnGuardCounterEnd;
    public event Action<Damage> OnAttack;
    public event Action<Damage> OnGetDamage;

    #region 외부 클래스
    private PlayerInterface playerInter;//체인지 세트 72 - 플레이어에 접근해서 가드나 피격 애니메이션 재생시키기 위한 변수
    private BattleDirector battleDirector;
    private PlayerSoundSet playerSoundSet = new PlayerSoundSet();
    private UISound uiSound = new UISound();
    BattlePresenter battlePresenter;
    private QTE qteEffect;
    private DefenseQTE defenseQTE;
    private EnemySignalUI enemySignalUI;
    #endregion
    #region 위치관련 변수
    private Transform playerTransform;
    private Vector2 battlePos;
    private Vector2 originalPosition;
    private float positionOffset = 4f;
    private Vector2 middlePos;
    private bool isMoveDone = false;
    // battlePos까지 이동을 마쳤는지?
    #endregion

    

    #region 프로퍼티
    public bool IsNoteChecked
    {
        get
        {
            if (isNoteChecked.Equals(null) || currentNoteIndex >= isNoteChecked.Count)
            {
                return true;
            }
            else
            {
                return isNoteChecked[currentNoteIndex];
            }
        }
    }
    public double JudgeStartTime { get { return judgeStartTime; } }
    public double JudgeEndTime { get { return judgeEndTime; } }
    public EnemyData Data { get { return data; } }
    public EnemyPattern[] EnemyPattern { get {return enemyPattern; } }
    public Transform Transform { get { return transform; } }
    public int HealthPoint { get => healthPoint; }
    public string EnemyName { get => enemyName; }
    public Vector2 Defense { get => new Vector2(slashDefense, pierceDefense); }
    public int Exp { get => exp; }
    #endregion

    #region 디버깅용!
    Image[] attackIcon;
    Image[] defenseIcon;
    #endregion

    private void OnEnable()//플레이버 텍스트 띄우기
    {
        GameObject flavorTextUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/FlavorText"));
        flavorTextUI.GetComponentInChildren<FlavorTextUI>().InitSetting(enemyImage,flavorTextName,enemyInfo);
    }

    #region 디스플레이부터 공격함수까지~
    public IEnumerator DisplayPattern(int[] patternArray)//패턴신호 + 공격위치로 이동//수리필요..
    {
        enemyAnimator.Dash();
        ResetSettings();
        this.patternArray = patternArray;
        patternLength = patternArray.Length;
        uiSound.SignalSound(gameObject,true);


        Metronome.instance.OnBeating += StartDisplay;

        yield return new WaitUntil(()=> isSignalEnd);
        Debug.Log($"{attackDelay}초 이후 시작");
        //yield return WaitForTargetedTime(attackDelay);
    }

    private void StartDisplay() {
        StartCoroutine(DisplayPatternSignal());
    }

    IEnumerator DisplayPatternSignal() {
        Metronome.instance.OnBeating -= StartDisplay;
        enemyAnimator.ReadyToPatternSignal();
        for (int i = 0; i < patternLength; ++i)
        {
            double targetTime = ((1f / patternArray[i]) * Const.QUARTER_NOTE) * stageManager.SecondsPerBeat;//패턴의 노트와 노트 사이의 시간
            targetTimes.Add(targetTime);                     //노트간 시간 모은 리스트
            isNoteChecked.Add(true);                         //리스트 공간 할당을 위한 더미 값                      
            yield return WaitForTargetedTime(targetTimes[i]);//패턴의 노트간 해당 시간만큼 대기
            uiSound.AttackSignal(gameObject, signalInstrument);
            enemySignalUI.AttackActive();
            enemyAnimator.PatternSignal();
        }
        
        defenseQTE.StartQTE();
        isSignalEnd = true;
    }

    public IEnumerator Attack()//수리필요
    {

        playerInter.PlayerAnimator.Guard();//체인지 세트 72

        double patternStartTime = Time.time;
        double sumOfTime = 0;
        uiSound.SignalSound(gameObject, true);
        for (int i = 0; i < patternLength; ++i)
        {
            stageManager.JudgeManager.EarlyCount = 0;
            isNoteChecked[i] = false;
            sumOfTime += targetTimes[i];
            judgeStartTime = patternStartTime + sumOfTime - targetTimes[i] * Const.BAD_JUDGE;//박자의 중간지점
            judgeEndTime = judgeStartTime + targetTimes[i] * 2 * Const.BAD_JUDGE;
            
            while (judgeEndTime >= Time.time)
            {
                OnFramePass?.Invoke();

                if (isNoteChecked[i])//패링했을 때
                {
                    enemyAnimator.Attack();
                    break;
                }
                yield return null;
            }
            if (!isNoteChecked[i])//패링 못했을 때
            {
                GiveDamage(new Judge(JudgeName.Miss));
                AkSoundEngine.PostEvent(enemyAttackSoundEvent, gameObject);
                enemyAnimator.Attack();
                Debug.Log($"{judgeEndTime}에 판정 종료");
            }
            
        }
        while (judgeEndTime >= Time.time)
        {
            yield return null; // 끝나는 시간을 항상 같게 하기 위해.
        }

        if (isEnteringGuardCounterPhase) // 가드카운터 턴으로 이사
        {
            //yield return EnterGuardCounterPhase();
        }
        
        yield return WaitForTargetedTime(attackDelay);

        #region 
        enemySignalUI.ResetIcon();
        #endregion
    }
    #endregion
    void SetBattlePos()
    {

        battlePos = stageManager.BattlePos;
        middlePos = (originalPosition + battlePos) / 2;

    }
    #region 이동관련 함수
    public void ReturnToOriginPos() {
        enemyAnimator.BackDash();
        transform.DOMove(originalPosition, 3).SetEase(Ease.OutQuart).OnComplete(()=> { enemyAnimator.Idle(); });
        
    }
    public void DashToBattlePos()
    {
        SetBattlePos();
        Debug.Log("DashToPlayer 진입");
        enemyAnimator.Dash();
        Metronome.instance.OnBeating += DashToHalf;
    }
    void DashToHalf()
    {
        transform.DOMove(middlePos, stageManager.SecondsPerBeat).SetEase(Ease.OutQuart).OnComplete(() => DashToDestination());
        Metronome.instance.OnBeating -= DashToHalf;
    }
    void DashToDestination()
    {
        transform.DOMove(new Vector3(battlePos.x + positionOffset, battlePos.y), stageManager.SecondsPerBeat).SetEase(Ease.OutQuart);
        isMoveDone = true;

    }

    #endregion
    private IEnumerator WaitForTargetedTime(double targetTime)
    {
        double elapsedTime = timeOffset; //초기값은 0

        while (targetTime >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timeOffset = elapsedTime - targetTime;
    }
    public void BindPattern(EnemyPattern[] enemyPattern) {
        this.enemyPattern = enemyPattern;
    }
    public void InitSettings(StageManager currentStageManager, Transform playerTransform) // StageManager에서 호출되는 초기세팅
    {

        #region 스테이지 매니저에서 참조
        
        stageManager = currentStageManager;
        enemySignalUI = currentStageManager.EnemySignalUI;
        attackIcon = currentStageManager.attackIcon;
        defenseIcon = currentStageManager.defenseIcon;
        battlePresenter = stageManager.BattlePresenter;
        float sPB = stageManager.SecondsPerBeat;
        defenseQTE = stageManager.DefenseQTE;
        defenseQTE.InitSetting(sPB);
        battlePos = stageManager.BattlePos;             //플레이어 위치
        playerInter = stageManager.PlayerInterface;
        #endregion
        #region 생성자
        battleDirector = new BattleDirector();
        targetTimes = new List<double>();               //타겟타임?
        isNoteChecked = new List<bool>();               //노트체크?
        enemyAnimator = GetComponent<EnemyAnimator>();
        #endregion

        attackDelay = Const.ATTACK_DELAY_BEATS * sPB;

        #region 플레이어
        this.playerTransform = playerTransform;
        originalPosition = transform.position; //원래 위치
        #endregion
        #region QTE 
        /*qteEffect = Instantiate(Resources.Load<GameObject>("Effect/QTE_Effect"))
            .GetComponent<QTE>();
        qteEffect.InitSettings(sPB);*/
        #endregion
        #region 디버깅!!!
        /*for (int i = 0; i < attackIcon.Length; ++i)
        {
            attackIcon[i].gameObject.SetActive(false);   //공격신호 비활성으로 초기화
        }*/
        enemySignalUI.ResetIcon();
        #endregion
    }

    private void ResetSettings()
    {

        targetTimes.Clear();
        isNoteChecked.Clear();
        currentNoteIndex = 0;
        timeOffset = 0;
        isMoveDone = false;
        isSignalEnd = false;

    }

    
#region 가드카운터 관련 - 별도의 턴으로 이동
    public void CheckCombo(int currentCombo, int maxGauge)
    {
        if (currentCombo >= maxGauge)
        {
            isEnteringGuardCounterPhase = true;
        }
    }
    public IEnumerator EnterGuardCounterPhase()
    {
        Debug.Log("가드 카운터!");
        yield return ProgressGuardCounterPhase(); //가드 카운터 시 진행
        isEnteringGuardCounterPhase = false;
        OnGuardCounterEnd?.Invoke();//가드카운터 게이지 리셋
    }
    private IEnumerator ProgressGuardCounterPhase()
    {
        Vector2 targetPosition = (playerTransform.position + transform.position) / 2;
        qteEffect.StartQTE(targetPosition);
        yield return WaitForTargetedTime(qteEffect.Speed);
    }
    #endregion

    #region 대미지 피격함수 클래스를 따로 파서 이사할 예정
    public void GiveDamage(Judge judge)//나중에 플레이어 클래스에서 GetDamage로 바꿔서 이사예정
    {
        Damage damage = new Damage(enemyAttack, judge.Name, new EnemyDamage());
        enemyAnimator.Attack();
        HandleParryJudge(judge);
        battlePresenter.EnemyToPlayer(damage);
        
    }
    
    public void GetDamage(Damage damage)// getdamage 메서드 자체를 이사시켜야될 듯
    {

        if (healthPoint == 0) { return; }
        if (damage.GetCalculatedDamage() <= 0) {
            enemyAnimator.Guard();
            return;
        }

        battleDirector.Shake(gameObject);
        enemyAnimator.Hurt();
        
        healthPoint -= damage.GetCalculatedDamage();
        
        OnGetDamage?.Invoke(damage);
        if (healthPoint <= 0)
        {
            healthPoint = 0;
            
            enemyAnimator.Die();
            stageManager.OnEnemyDie();
        }
    }
    
    #endregion
    public void HandleParryJudge(Judge judge)//패링 판정 다루는 함수
    {
        
        if (judge.Name.Equals(JudgeName.Miss))
        {
            playerInter.PlayerAnimator.Hurt();
            enemySignalUI.DefenseActive(judge);
            isNoteChecked[currentNoteIndex] = false;
            ++currentNoteIndex;
            return;
        }
        else if(judge.Name.Equals(JudgeName.Perfect))
        {
            playerSoundSet.PerfectParrySound(gameObject);
            battleDirector.CameraShake(0.3f, 1, 100, 30);
            playerInter.PlayerAnimator.Parry(true);
            
        }
        else
        {
            playerSoundSet.PerfectParrySound(gameObject);
            battleDirector.CameraShake(0.3f, 0.5f, 100, 30);
            playerInter.PlayerAnimator.Parry(false);
           
        }
        enemySignalUI.DefenseActive(judge);
        isNoteChecked[currentNoteIndex] = true;
        ++currentNoteIndex;
    }

    public Vector2 GetDefense() {
        return new Vector2(slashDefense, pierceDefense);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void StopActions() => StopAllCoroutines();
}
