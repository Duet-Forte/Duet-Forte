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


    #region �⺻���� �������ͽ�
    [Header("Enemy Info")]
    [Space(5f)]
    [Tooltip("����� �̸�, hitparticle�� �����ǰ� �̸��� ���ƾ� �Ѵ�.")]
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
    /// ������ ��������� ���
    /// slash ���� ���� �����= slashAttack - slashDefense (���� ������� �׻� 0���� ũ�ų� ����)
    /// </summary>
    [SerializeField] protected float slashDefense;
    [SerializeField] protected float pierceDefense;
    [Header("Sounds")]
    [Space(5f)]
    [SerializeField] protected string enemyAttackSoundEvent;
    [SerializeField] protected string enemyHitSoundEvent;
    [SerializeField] protected SignalInstrument signalInstrument; //�ñ׳� ���� ����
    [Space(10f)]
    #endregion


    protected double attackDelay;
    protected double timeOffset;
    protected int currentNoteIndex;

    protected List<double> targetTimes;
    protected List<bool> isNoteChecked; //���� �ϳ��� ���� ���� ����Ʈ

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

    #region �ܺ� Ŭ����
    protected PlayerInterface playerInter;//ü���� ��Ʈ 72 - �÷��̾ �����ؼ� ���峪 �ǰ� �ִϸ��̼� �����Ű�� ���� ����
    protected BattleDirector battleDirector;
    protected PlayerSoundSet playerSoundSet = new PlayerSoundSet();
    protected UISound uiSound = new UISound();
    protected BattlePresenter battlePresenter;
    protected QTE qteEffect;
    protected DefenseQTE defenseQTE;
    protected EnemySignalUI enemySignalUI;
    protected BuffManager buffManager;
    #endregion
    #region ��ġ���� ����
    protected Transform playerTransform;
    protected Vector2 battlePos;
    protected Vector2 originalPosition;
    protected float positionOffset = 4f;
    protected Vector2 middlePos;
    protected bool isMoveDone = false;
    // battlePos���� �̵��� ���ƴ���?
    #endregion

    

    #region ������Ƽ
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

    #region ������!
    Image[] attackIcon;
    Image[] defenseIcon;
    #endregion

    protected void OnEnable()//�÷��̹� �ؽ�Ʈ ����
    {
        GameObject flavorTextUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/FlavorText"));
        flavorTextUI.GetComponentInChildren<FlavorTextUI>().InitSetting(enemyImage,flavorTextName,enemyInfo);
    }

    #region ���÷��̺��� �����Լ�����~
    public IEnumerator DisplayPattern(int[] patternArray)//���Ͻ�ȣ + ������ġ�� �̵�//�����ʿ�..
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
            double targetTime = ((1f / patternArray[i]) * Const.QUARTER_NOTE) * stageManager.SecondsPerBeat;//������ ��Ʈ�� ��Ʈ ������ �ð�
            targetTimes.Add(targetTime);                     //��Ʈ�� �ð� ���� ����Ʈ
            isNoteChecked.Add(true);                         //����Ʈ ���� �Ҵ��� ���� ���� ��                      
            yield return WaitForTargetedTime(targetTimes[i]);//������ ��Ʈ�� �ش� �ð���ŭ ���
            uiSound.AttackSignal(gameObject, signalInstrument);
            enemySignalUI.AttackActive();
            enemyAnimator.PatternSignal();
        }
        
        defenseQTE.StartQTE();
        isSignalEnd = true;
    }

    public IEnumerator Attack()//�����ʿ�
    {
        //debug
        double tmp=0;
        double tmp2 = 0;
        playerInter.PlayerAnimator.Guard();//ü���� ��Ʈ 72

        double patternStartTime = Time.time;
        double sumOfTime = 0;
        uiSound.SignalSound(gameObject, true);
        for (int i = 0; i < patternLength; ++i)
        {
            stageManager.JudgeManager.EarlyCount = 0;
            isNoteChecked[i] = false;
            sumOfTime += targetTimes[i];
            judgeStartTime = patternStartTime + sumOfTime - (stageManager.SecondsPerBeat * 0.5d);//Ÿ��Ÿ���� �������� �Ǵ� ����
            judgeEndTime = judgeStartTime + stageManager.SecondsPerBeat * 0.75d;//Ÿ��Ÿ���� 25�ۼ�Ʈ �Ŀ� �Ǵ� ��
            while (judgeEndTime >= Time.time)
            {
                OnFramePass?.Invoke();

                if (isNoteChecked[i])//�и����� ��
                {
                    enemyAnimator.Attack();
                    break;
                }
                yield return null;
            }
            if (!isNoteChecked[i])//�и� ������ ��
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
            yield return null; // ������ �ð��� �׻� ���� �ϱ� ����.
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
    #region �̵����� �Լ�
    public void ReturnToOriginPos() { 
        enemyAnimator.BackDash();

        //�� ����
        OnTurnEnd?.Invoke();

        transform.DOMove(originalPosition, 3).SetEase(Ease.OutQuart).OnComplete(()=> { enemyAnimator.Idle(); });
        
    }
    public void DashToBattlePos()
    {
        SetBattlePos();
        Debug.Log("DashToPlayer ����");
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
        double elapsedTime = timeOffset; //�ʱⰪ�� 0

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
    public virtual void InitSettings(StageManager currentStageManager, Transform playerTransform) // StageManager���� ȣ��Ǵ� �ʱ⼼��
    {

        #region �������� �Ŵ������� ����
        
        stageManager = currentStageManager;
        enemySignalUI = currentStageManager.EnemySignalUI;
        attackIcon = currentStageManager.attackIcon;
        defenseIcon = currentStageManager.defenseIcon;
        battlePresenter = stageManager.BattlePresenter;
        double sPB = stageManager.SecondsPerBeat;
        defenseQTE = stageManager.DefenseQTE;
        defenseQTE.InitSetting(sPB);
        battlePos = stageManager.BattlePos;             //�÷��̾� ��ġ
        playerInter = stageManager.PlayerInterface;

        currentHP = maxHP;
        #endregion
        #region ������
        battleDirector = new BattleDirector();
        targetTimes = new List<double>();               //Ÿ��Ÿ��?
        isNoteChecked = new List<bool>();               //��Ʈüũ?
        enemyAnimator = GetComponent<EnemyAnimator>();

        ///������
        buffManager=new BuffManager();
        ///
        #endregion

        attackDelay = Const.ATTACK_DELAY_BEATS * sPB;

        #region �÷��̾�
        this.playerTransform = playerTransform;
        originalPosition = transform.position; //���� ��ġ
        #endregion
        #region ���ݽ�ȣ
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

    
#region ����ī���� ���� - ������ ������ �̵�
    public void CheckCombo(int currentCombo, int maxGauge)
    {
        if (currentCombo >= maxGauge)
        {
            isEnteringGuardCounterPhase = true;
        }
    }
    #endregion

    #region ����� �ǰ��Լ� Ŭ������ ���� �ļ� �̻��� ����
    public void GiveDamage(Judge judge)//���߿� �÷��̾� Ŭ�������� GetDamage�� �ٲ㼭 �̻翹��
    {
        Damage damage = new Damage(enemyAttack, judge.Name, new EnemyDamage());
        enemyAnimator.Attack();
        HandleParryJudge(judge);
        battlePresenter.EnemyToPlayer(damage);
        
    }
    
    public void GetDamage(Damage damage)// �ǰ� ���� Ŭ������ ������ �ǳ�...
    {

        if (currentHP == 0) { return; }
        if (damage.GetCalculatedDamage() <= 0) {
            enemyAnimator.Guard();
            return;
        }

        battleDirector.Shake(gameObject);
        enemyAnimator.Hurt();
        
        currentHP -= damage.GetCalculatedDamage();

        AkSoundEngine.PostEvent(enemyHitSoundEvent, gameObject); //���� ���۵Ǹ� enemyHitSoundEvent �ֱ�

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
    public void HandleParryJudge(Judge judge)//�и� ���� �ٷ�� �Լ�
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
