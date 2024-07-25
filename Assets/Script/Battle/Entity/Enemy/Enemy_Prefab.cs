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
    protected StageManager stageManager;
    protected EnemyData data;
    protected EnemyPattern[] enemyPattern;
    protected EnemyAnimator enemyAnimator;


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
    protected int currentHP;
    [SerializeField] protected int maxHP;
    [SerializeField] protected float enemyAttack;
    [SerializeField] protected int exp;
    /// <summary>
    /// 방어력은 빼기공식을 사용
    /// slash 피해 순수 대미지= slashAttack - slashDefense (순수 대미지는 항상 0보다 크거나 같음)
    /// </summary>
    [SerializeField] protected float slashDefense;
    [SerializeField] protected float pierceDefense;
    [Header("Sounds")]
    [Space(5f)]
    [SerializeField] protected string enemyAttackSoundEvent;
    [SerializeField] protected string enemyHitSoundEvent;
    [SerializeField] protected SignalInstrument signalInstrument; //시그널 사운드 고르기
    [Space(10f)]
    #endregion


    protected double attackDelay;
    protected double timeOffset;
    protected int currentNoteIndex;

    protected List<double> targetTimes;
    protected List<bool> isNoteChecked; //공격 하나당 판정 여부 리스트

    protected double judgeStartTime;
    protected double judgeEndTime;
    protected int patternLength;
    protected int[] patternArray;
    protected bool isEnteringGuardCounterPhase;
    protected bool isSignalEnd=false;
    

    public event Action OnFramePass;
    public event Action OnGuardCounterEnd;
    public event Action<Damage> OnAttack;
    public event Action<Damage> OnGetDamage;
    public event Action OnHit;
    public event Action OnTurnEnd;

    #region 외부 클래스
    protected PlayerInterface playerInter;//체인지 세트 72 - 플레이어에 접근해서 가드나 피격 애니메이션 재생시키기 위한 변수
    protected BattleDirector battleDirector;
    protected PlayerSoundSet playerSoundSet = new PlayerSoundSet();
    protected UISound uiSound = new UISound();
    protected BattlePresenter battlePresenter;
    protected QTE qteEffect;
    protected DefenseQTE defenseQTE;
    protected EnemySignalUI enemySignalUI;
    protected BuffManager buffManager;
    #endregion
    #region 위치관련 변수
    protected Transform playerTransform;
    protected Vector2 battlePos;
    protected Vector2 originalPosition;
    protected float positionOffset = 4f;
    protected Vector2 middlePos;
    protected bool isMoveDone = false;
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
    public int CurrentHP { get => currentHP; }
    public int MaxHP { get => maxHP; }
    public string EnemyName { get => enemyName; }
    public Vector2 Defense { get => new Vector2(slashDefense, pierceDefense); }
    public int Exp { get => exp; }
    #endregion

    #region 디버깅용!
    Image[] attackIcon;
    Image[] defenseIcon;
    #endregion

    protected void OnEnable()//플레이버 텍스트 띄우기
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
        //yield return WaitForTargetedTime(attackDelay);
    }

    protected void StartDisplay() { 
        Metronome.instance.OnBeating -= StartDisplay;
        StartCoroutine(DisplayPatternSignal());
    }

    IEnumerator DisplayPatternSignal() {
       
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
        //debug
        double tmp=0;
        double tmp2 = 0;
        playerInter.PlayerAnimator.Guard();//체인지 세트 72

        double patternStartTime = Time.time;
        double sumOfTime = 0;
        uiSound.SignalSound(gameObject, true);
        for (int i = 0; i < patternLength; ++i)
        {
            stageManager.JudgeManager.EarlyCount = 0;
            isNoteChecked[i] = false;
            sumOfTime += targetTimes[i];
            judgeStartTime = patternStartTime + sumOfTime - (stageManager.SecondsPerBeat * 0.5d);//타겟타임의 절반전에 판단 시작
            judgeEndTime = judgeStartTime + stageManager.SecondsPerBeat * 0.75d;//타겟타임의 25퍼센트 후에 판단 끝
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
            }
            tmp = judgeEndTime;
            tmp2= targetTimes[i]*0.5d;
        }
        while (judgeEndTime >= Time.time)
        {
            yield return null; // 끝나는 시간을 항상 같게 하기 위해.
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

        //턴 종료
        OnTurnEnd?.Invoke();

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
        transform.DOMove(middlePos, (float)stageManager.SecondsPerBeat).SetEase(Ease.OutQuart).OnComplete(() => DashToDestination());
        Metronome.instance.OnBeating -= DashToHalf;
    }
    void DashToDestination()
    {
        transform.DOMove(new Vector3(battlePos.x + positionOffset, battlePos.y), (float)stageManager.SecondsPerBeat).SetEase(Ease.OutQuart);
        isMoveDone = true;

    }

    #endregion
    protected IEnumerator WaitForTargetedTime(double targetTime)
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
    public virtual void InitSettings(StageManager currentStageManager, Transform playerTransform) // StageManager에서 호출되는 초기세팅
    {

        #region 스테이지 매니저에서 참조
        
        stageManager = currentStageManager;
        enemySignalUI = currentStageManager.EnemySignalUI;
        attackIcon = currentStageManager.attackIcon;
        defenseIcon = currentStageManager.defenseIcon;
        battlePresenter = stageManager.BattlePresenter;
        double sPB = stageManager.SecondsPerBeat;
        defenseQTE = stageManager.DefenseQTE;
        defenseQTE.InitSetting(sPB);
        battlePos = stageManager.BattlePos;             //플레이어 위치
        playerInter = stageManager.PlayerInterface;

        currentHP = maxHP;
        #endregion
        #region 생성자
        battleDirector = new BattleDirector();
        targetTimes = new List<double>();               //타겟타임?
        isNoteChecked = new List<bool>();               //노트체크?
        enemyAnimator = GetComponent<EnemyAnimator>();

        ///디버깅용
        buffManager=new BuffManager();
        ///
        #endregion

        attackDelay = Const.ATTACK_DELAY_BEATS * sPB;

        #region 플레이어
        this.playerTransform = playerTransform;
        originalPosition = transform.position; //원래 위치
        #endregion
        #region 공격신호
        enemySignalUI.ResetIcon();
        #endregion
    }

    protected void ResetSettings()
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
    #endregion

    #region 대미지 피격함수 클래스를 따로 파서 이사할 예정
    public void GiveDamage(Judge judge)//나중에 플레이어 클래스에서 GetDamage로 바꿔서 이사예정
    {
        Damage damage = new Damage(enemyAttack, judge.Name, new EnemyDamage());
        enemyAnimator.Attack();
        HandleParryJudge(judge);
        battlePresenter.EnemyToPlayer(damage);
        
    }
    
    public void GetDamage(Damage damage)// 피격 전용 클래스를 만들어야 되나...
    {

        if (currentHP == 0) { return; }
        if (damage.GetCalculatedDamage() <= 0) {
            enemyAnimator.Guard();
            return;
        }

        battleDirector.Shake(gameObject);
        enemyAnimator.Hurt();
        
        currentHP -= damage.GetCalculatedDamage();

        AkSoundEngine.PostEvent(enemyHitSoundEvent, gameObject); //사운드 제작되면 enemyHitSoundEvent 넣기

        OnGetDamage?.Invoke(damage);
        OnHit?.Invoke();
        if (currentHP <= 0)
        {
            currentHP = 0;
            
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
    protected void OnDestroy()
    {
        StopAllCoroutines();
    }
    public void AddBuff(IBuff buff) { 
    
        buffManager.AddBuff(buff);
    }
    public void DeleteBuff(IBuff buff) { }
    public void StopActions() => StopAllCoroutines();
}
