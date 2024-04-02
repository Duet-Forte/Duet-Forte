using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.CustomEnum;
using UnityEngine.UI;
using SoundSet;

using Director;
public class Enemy_Prefab : MonoBehaviour, IEnemy
{
    private StageManager stageManager;
    private EnemyData data;
    private EnemyPattern[] enemyPattern;
    private EnemyAnimator enemyAnimator;


    #region �⺻���� �������ͽ�
    [Header("Entity Stats")]
    [Space(10f)]
    [SerializeField] string enemyName;
    [SerializeField] private int healthPoint;
    [SerializeField] private float enemyAttack;
    /// <summary>
    /// ������ ��������� ���
    /// slash ���� ���� �����= slashAttack - slashDefense (���� ������� �׻� 0���� ũ�ų� ����)
    /// </summary>
    [SerializeField] private float slashDefense;
    [SerializeField] private float pierceDefense;
    [Space(10f)]
    #endregion
    
    
    private float attackDelay;
    private double timeOffset;
    private int currentNoteIndex;

    private List<double> targetTimes;
    private List<bool> isNoteChecked; //���� �ϳ��� ���� ���� ����Ʈ

    private double judgeStartTime;
    private double judgeEndTime;
    private int patternLength;
    private int[] patternArray;
    private bool isEnteringGuardCounterPhase;
    private bool isSignalEnd=false;
    

    public event Action OnFramePass;
    public event Action OnGuardCounterEnd;
    public event Action<int> OnAttack;
    public event Action<int> OnGetDamage;

    #region �ܺ� Ŭ����
    private PlayerInterface playerInter;//ü���� ��Ʈ 72 - �÷��̾ �����ؼ� ���峪 �ǰ� �ִϸ��̼� �����Ű�� ���� ����
    private BattleDirector battleDirector;
    private PlayerSoundSet playerSoundSet = new PlayerSoundSet();
    private UISound uiSound = new UISound();
    BattlePresenter battlePresenter;
    private QTE qteEffect;
    private DefenseQTE defenseQTE;
    private EnemySignalUI enemySignalUI;
    #endregion
    #region ��ġ���� ����
    private Transform playerTransform;
    private Vector2 battlePos;
    private Vector2 originalPosition;
    private float positionOffset = 4f;
    private Vector2 middlePos;
    private bool isMoveDone = false;
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
    public int HealthPoint { get => healthPoint; }
    public string EnemyName { get => enemyName; }
    public Vector2 Defense { get => new Vector2(slashDefense, pierceDefense); }
    #endregion

    #region ������!
    Image[] attackIcon;
    Image[] defenseIcon;
    #endregion

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
        Debug.Log($"{attackDelay}�� ���� ����");
        //yield return WaitForTargetedTime(attackDelay);
    }

    private void StartDisplay() {
        StartCoroutine(DisplayPatternSignal());
    }

    IEnumerator DisplayPatternSignal() {
        Metronome.instance.OnBeating -= StartDisplay;
        for (int i = 0; i < patternLength; ++i)
        {
            double targetTime = ((1f / patternArray[i]) * Const.QUARTER_NOTE) * stageManager.SecondsPerBeat;//������ ��Ʈ�� ��Ʈ ������ �ð�
            targetTimes.Add(targetTime);                     //��Ʈ�� �ð� ���� ����Ʈ
            isNoteChecked.Add(true);                         //����Ʈ ���� �Ҵ��� ���� ���� ��                      
            yield return WaitForTargetedTime(targetTimes[i]);//������ ��Ʈ�� �ش� �ð���ŭ ���
            uiSound.SignalSound(gameObject, false);
            enemySignalUI.AttackActive();
        }
        
        defenseQTE.StartQTE();
        isSignalEnd = true;
    }

    public IEnumerator Attack()//�����ʿ�
    {
        playerInter.PlayerAnimator.Guard();//ü���� ��Ʈ 72

        double patternStartTime = Time.time;
        double sumOfTime = 0;
        uiSound.SignalSound(gameObject, true);
        for (int i = 0; i < patternLength; ++i)
        {
            stageManager.JudgeManager.EarlyCount = 0;
            isNoteChecked[i] = false;
            sumOfTime += targetTimes[i];
            judgeStartTime = patternStartTime + sumOfTime - targetTimes[i] * Const.BAD_JUDGE;
            judgeEndTime = judgeStartTime + targetTimes[i] * 2 * Const.BAD_JUDGE;
            
            while (judgeEndTime >= Time.time)
            {
                OnFramePass?.Invoke();

                if (isNoteChecked[i])
                {
                    enemyAnimator.Attack();
                    break;
                }
                yield return null;
            }
            if (!isNoteChecked[i])
            {
                GiveDamage();
                enemyAnimator.Attack();
                Debug.Log($"{judgeEndTime}�� ���� ����");
            }
        }
        while (judgeEndTime >= Time.time)
        {
            yield return null; // ������ �ð��� �׻� ���� �ϱ� ����.
        }

        if (isEnteringGuardCounterPhase) // ����ī���� ������ �̻�
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
    #region �̵����� �Լ�
    public void ReturnToOriginPos() {
        enemyAnimator.BackDash();
        transform.DOMove(originalPosition, stageManager.SecondsPerBeat).SetEase(Ease.OutQuart).OnComplete(()=>enemyAnimator.Idle());
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
        transform.DOMove(middlePos, stageManager.SecondsPerBeat).SetEase(Ease.OutQuart).OnComplete(() => DashToDestination());
        Metronome.instance.OnBeating -= DashToHalf;
    }
    void DashToDestination()
    {
        transform.DOMove(new Vector3(battlePos.x + positionOffset, battlePos.y), stageManager.SecondsPerBeat).SetEase(Ease.OutQuart);
        isMoveDone = true;

    }

    #endregion
    private IEnumerator WaitForTargetedTime(double targetTime)// ��Ʈ�γ� OnBeat�����ϸ� �ʿ������ ��
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
    public void InitSettings(StageManager currentStageManager, Transform playerTransform) // StageManager���� ȣ��Ǵ� �ʱ⼼��
    {

        #region �������� �Ŵ������� ����
        enemyName = gameObject.name;
        stageManager = currentStageManager;
        enemySignalUI = currentStageManager.EnemySignalUI;
        attackIcon = currentStageManager.attackIcon;
        defenseIcon = currentStageManager.defenseIcon;
        battlePresenter = stageManager.BattlePresenter;
        float sPB = stageManager.SecondsPerBeat;
        defenseQTE = stageManager.DefenseQTE;
        defenseQTE.InitSetting(sPB);
        battlePos = stageManager.BattlePos;             //�÷��̾� ��ġ
        playerInter = stageManager.PlayerInterface;
        #endregion
        #region ������
        battleDirector = new BattleDirector();
        targetTimes = new List<double>();               //Ÿ��Ÿ��?
        isNoteChecked = new List<bool>();               //��Ʈüũ?
        enemyAnimator = GetComponent<EnemyAnimator>();
        #endregion

        attackDelay = Const.ATTACK_DELAY_BEATS * sPB;

        #region �÷��̾�
        this.playerTransform = playerTransform;
        originalPosition = transform.position; //���� ��ġ
        #endregion
        #region QTE 
        /*qteEffect = Instantiate(Resources.Load<GameObject>("Effect/QTE_Effect"))
            .GetComponent<QTE>();
        qteEffect.InitSettings(sPB);*/
        #endregion
        #region �����!!!
        /*for (int i = 0; i < attackIcon.Length; ++i)
        {
            attackIcon[i].gameObject.SetActive(false);   //���ݽ�ȣ ��Ȱ������ �ʱ�ȭ
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

    
#region ����ī���� ���� - ������ ������ �̵�
    public void CheckCombo(int currentCombo, int maxGauge)
    {
        if (currentCombo >= maxGauge)
        {
            isEnteringGuardCounterPhase = true;
        }
    }
    public IEnumerator EnterGuardCounterPhase()
    {
        Debug.Log("���� ī����!");
        yield return ProgressGuardCounterPhase(); //���� ī���� �� ����
        isEnteringGuardCounterPhase = false;
        OnGuardCounterEnd?.Invoke();//����ī���� ������ ����
    }
    private IEnumerator ProgressGuardCounterPhase()
    {
        Vector2 targetPosition = (playerTransform.position + transform.position) / 2;
        qteEffect.StartQTE(targetPosition);
        yield return WaitForTargetedTime(qteEffect.Speed);
    }
    #endregion

    #region ����� �ǰ��Լ� Ŭ������ ���� �ļ� �̻��� ����
    public void GiveDamage()//���߿� �÷��̾� Ŭ�������� GetDamage�� �ٲ㼭 �̻翹��
    {
        int damage = 1; // �ӽ������� 1������ ��.
        enemyAnimator.Attack();
        Judge judge = new Judge();
        judge.Name = JudgeName.Miss;
        HandleParryJudge(judge);
        OnAttack?.Invoke(damage);
        battlePresenter.EnemyToPlayer(damage);
        
    }
    
    public void GetDamage(int playerAttack)// getdamage �޼��� ��ü�� �̻���Ѿߵ� ��
    {
        if (playerAttack <= 0) {
            enemyAnimator.Guard();
            return;
        }

        battleDirector.Shake(gameObject);
        enemyAnimator.Hurt();

        healthPoint -= playerAttack;
        OnGetDamage?.Invoke(playerAttack);
        if (healthPoint <= 0)
            stageManager.OnEnemyDie();
    }
    /*public void GetDamage(int playerAttack) {//����ī���� �ǰ�
        if (playerAttack <= 0)
        {
            enemyAnimator.Guard();
            return;
        }

        battleDirector.Shake(gameObject);
        healthPoint -= playerAttack;
        //�ǰ� �ִϸ��̼�
        OnGetDamage?.Invoke(playerAttack);
        if (healthPoint <= 0)
            stageManager.OnEnemyDie();
    }*/
    #endregion
    public void HandleParryJudge(Judge judge, int damage = 0)//�и� ���� �ٷ�� �Լ�
    {
        
        if (judge.Name.Equals(JudgeName.Miss))
        {
            //AkSoundEngine.PostEvent("Miss", gameObject);
            playerInter.PlayerAnimator.Hurt(); 
        }
        else if(judge.Name.Equals(JudgeName.Perfect))
        {
            //AkSoundEngine.PostEvent("Parry_Sound", gameObject);
            playerSoundSet.PerfectParrySound(gameObject);
            //AkSoundEngine.PostEvent("Player_Parry_SFX", gameObject);
            playerInter.PlayerAnimator.Parry(true);
            //ü���� ��Ʈ 72
        }
        else
        {
            //AkSoundEngine.PostEvent("Parry_Sound", gameObject);
            playerSoundSet.PerfectParrySound(gameObject);
            //AkSoundEngine.PostEvent("Player_Parry_SFX", gameObject);
            playerInter.PlayerAnimator.Parry(false);
            //ü���� ��Ʈ 72
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
