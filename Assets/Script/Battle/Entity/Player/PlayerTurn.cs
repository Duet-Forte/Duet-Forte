using System.Collections;
using UnityEngine;
using DG.Tweening;
using Util.CustomEnum;
using Cinemachine;
using Director;
using SoundSet;
using Unity.VisualScripting;
using System;
//using Cysharp.Threading.Tasks;

public class PlayerTurn : MonoBehaviour ,ITurnHandler
{
    //공격신호
    //비공격 제한시간
    //플레이어 공격시도 여부
    //
    // Start is called before the first frame update

    private StageManager stageManager;
    private PlayerAnimator playerAnimator;
    private PlayerStatus playerStatus;  //플레이어 스탯
    private bool isTurnOver;

    public event Action<bool> onBasicAttack;
    
    #region 대기 시간 변수
    private double halfBeat;
    private int waitForAttackDelay = 1;//접근 후 공격신호가 나올 때까지의 시간(박자)
    private int noAttackDelay = 8;//접근후 공격을 안하고 최대로 대기하는 시간(박자)
    private int delayBeatCount = 8;//대기할 박자 위 변수들로 초기화되는 변수
    #endregion
    [SerializeField] GameObject playerAttackSignal;
  //[SerializeField] private bool canAttack = false; //공격 가능한지?
    
    int dashDuringTime = 1;//대쉬해서 적한테 접근하는 시간 단위는 초
    #region 위치관련 변수
    
    Vector2 originPlayerPos;//플레이어 원래 있는 위치
    Vector2 battlePos;
    Vector2 middlePos;
    private float positionOffset=-7f;
    private bool isMoveDone=false; // battlePos까지 이동을 마쳤는지?

    public Vector2 BattlePos { get => battlePos; }
    #endregion

    #region 외부클래스
    private CinemachineImpulseSource cinemachineImpulseSource;
    private BattleDirector battleDirector = new BattleDirector();
    UISound uiSound = new UISound();
    private BattlePresenter battlePresenter;
    #endregion
    public bool IsMoveDone { get => isMoveDone; }
    PlayerAttack  playerAttack;
    private int actionCase = 0;
    public void InitSettings(StageManager stageManager, Transform enemyTransform)
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerStatus = GetComponent<PlayerStatus>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        //playerStatus.InitSetting();
        
        this.stageManager = stageManager;
        battlePresenter = this.stageManager.BattlePresenter;
        originPlayerPos = gameObject.transform.position;
        
    } 
    void SetBattlePos() { 

        battlePos = stageManager.BattlePos;
        middlePos = (originPlayerPos + battlePos) / 2;
    
    }
    public IEnumerator TurnStart()
    {
        Debug.Log("플레이어 턴 시작");
        ResetSettings();
        ActionSwitch(0);
        while (!isTurnOver)
        {
            yield return null;
        }
    }
    void ResetSettings() {

        isTurnOver = false;
        isMoveDone = false;
        actionCase = 0;
    }
    public IEnumerator TurnEnd()
    {
        //playerAnimator.Idle();
        actionCase = 0;
        stageManager.CurrentTurn = Turn.EnemyTurn;
        Debug.Log("플레이어 턴 끝");
        yield return null;
    }
    void ActionSwitch(int actionCase)
    { //플레이어 턴에서 한 사이클의 행동(직관적으로 보기위함과 혹시나 한 턴에 행동을 두번 하게 될 수도 있을 거 같아서 스위치문 사용)

        switch (actionCase)
        {
           
            case 0:
                StartCoroutine(WaitForAnAttack()); ; break;
            case 1:
                StartCoroutine(ConsiderAttacking()); break;
            case 2:
                break;

        }


    }

    

    #region 이동 함수
    public void DashToBattlePos() {
        SetBattlePos();
        Debug.Log(battlePos);
        Debug.Log("DashToEnemy 진입");
        playerAnimator.Dash();
        Metronome.instance.OnBeating += DashToHalf;
    }
    void DashToHalf() {
        playerAnimator.Particle_DashDust();
        transform.DOMove(middlePos, (float)stageManager.SecondsPerBeat).SetEase(Ease.OutQuart).OnComplete(() => DashToDestination());
        Metronome.instance.OnBeating -= DashToHalf;
    }
    void DashToDestination() {
        playerAnimator.Particle_DashDust();
        transform.DOMove(new Vector3(battlePos.x + positionOffset, battlePos.y), (float)stageManager.SecondsPerBeat).SetEase(Ease.OutQuart); //ActionStartTurn에서 호출될 때는 actionCase증가시키는거 빼야함..OnComplete(() => ActionSwitch(++actionCase)
        Debug.Log("이동하려는 battlePos : "+battlePos);
        isMoveDone = true;
    }


    public void ReturnToOriginPos() {
        playerAnimator.BackDash();
        transform.DOMove(originPlayerPos, (float)Metronome.instance.SecondsPerBeat*3).SetEase(Ease.OutExpo).OnComplete(()=>playerAnimator.Idle());
                
    }
    #endregion

    void DecreaseOnBeat()
    {//OnBeating에 구독해서 사용하는 카운트 다운
        delayBeatCount--;
    }
    void AttackCountDown() {
        playerAnimator.attackCountDown();
        uiSound.SignalSound(gameObject, false); //공격 전조 신호
            }

    IEnumerator WaitForAnAttack()
    { //적한테 접근하고 공격하기 전까지 기다리기
        Debug.Log("WaitForAnAttacking 진입");
        delayBeatCount = waitForAttackDelay;
        Metronome.instance.OnBeating += DecreaseOnBeat; //비트마다 호출되는 OnBeating에 구독시켜서 비트마다 delayBeatCount감소
        Metronome.instance.OnBeating += AttackCountDown;
        while (true)
        {
            if (delayBeatCount == 1) {
                Metronome.instance.OnBeating -= AttackCountDown; //공격신호의 신호 종료 
            }
            if (delayBeatCount == 0)
            { // delayBeatCount만큼의 박자 뒤에 공격신호
                Metronome.instance.OnBeating -= DecreaseOnBeat;// 구독해제
                //StartCoroutine(AttackSignal());//공격신호 파티클재생
                playerAnimator.attackSignalPlay();
                uiSound.SignalSound(gameObject, true); //공격 시작 신호
                break;
            }
            yield return null;
        }
        halfBeat = Metronome.instance.SecondsPerBeat*0.5;
        while (Metronome.instance.CurrentTime < halfBeat) {
            
            yield return null;
        }
                
                ActionSwitch(++actionCase);//ConsiderAttacking
         

    }
    IEnumerator ConsiderAttacking()
    {
        AttackSignal();
        Debug.Log("ConsiderAttacking 진입");
        delayBeatCount = noAttackDelay;// 4박자 동안 플레이어 턴 유지
        Debug.Log("delayBeatCount : "+delayBeatCount);
        Metronome.instance.OnBeating += DecreaseOnBeat;
        onBasicAttack?.Invoke(true);
        while (true)//플레이어 턴 제한시간 4박자
        {

            //분기점 - 공격했을 시 제한시간 리셋 
            if (Input.GetKeyDown(KeyCode.F))
            {
                onBasicAttack?.Invoke(false);
                Metronome.instance.OnBeating -= DecreaseOnBeat;//공격 안하고 대기하는 동안의 제한시간은 사라짐
                yield return StartCoroutine(playerAttack.StartAttack());
                
                break;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                onBasicAttack?.Invoke(false);
                Metronome.instance.OnBeating -= DecreaseOnBeat;//공격 안하고 대기하는 동안의 제한시간은 사라짐
                yield return StartCoroutine(playerAttack.StartAttack());
                break;
            }
            if (delayBeatCount == 0)//제한시간 오버
            {
                Metronome.instance.OnBeating -= DecreaseOnBeat;//delayBeatCount가 0이 돼서 구독취소
                Debug.Log("제한시간 오버");
                //transform.DOMove(originPlayerPos, dashDuringTime).SetEase(Ease.OutQuart).OnComplete(PlayerTurnEnd);//ReturnToOriginPos로 통합됨
                playerAnimator.Guard();
                PlayerTurnEnd();
                break;

            }
            yield return null;
        }
        //공격 후 턴 종료

        playerAnimator.Guard();
        PlayerTurnEnd();
        yield break;

    }


    IEnumerator AttackSignal()
    {
        Debug.Log("공격신호");   //플레이어가 공격하기 전 박자에 알려주는 신호 EnemyTurn의 공격신호와 같은 역할

        playerAnimator.attackSignalPlay();
        yield return null;
    }
    public void cineMachineImpulse() {

        cinemachineImpulseSource.GenerateImpulse();
    }
    void PlayerTurnEnd()
    {
        onBasicAttack?.Invoke(false);
        isTurnOver = true;
    }

    public void StopActions() {
        playerAnimator.Idle();
        StopAllCoroutines();
    
    }


   
    
}
