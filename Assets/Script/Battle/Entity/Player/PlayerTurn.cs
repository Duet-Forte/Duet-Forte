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
    //���ݽ�ȣ
    //����� ���ѽð�
    //�÷��̾� ���ݽõ� ����
    //
    // Start is called before the first frame update

    private StageManager stageManager;
    private PlayerAnimator playerAnimator;
    private PlayerStatus playerStatus;  //�÷��̾� ����
    private bool isTurnOver;

    public event Action<bool> onBasicAttack;
    
    #region ��� �ð� ����
    private double halfBeat;
    private int waitForAttackDelay = 1;//���� �� ���ݽ�ȣ�� ���� �������� �ð�(����)
    private int noAttackDelay = 8;//������ ������ ���ϰ� �ִ�� ����ϴ� �ð�(����)
    private int delayBeatCount = 8;//����� ���� �� ������� �ʱ�ȭ�Ǵ� ����
    #endregion
    [SerializeField] GameObject playerAttackSignal;
  //[SerializeField] private bool canAttack = false; //���� ��������?
    
    int dashDuringTime = 3;//�뽬�ؼ� ������ �����ϴ� �ð� ������ ��Ʈ
    #region ��ġ���� ����
    
    Vector2 originPlayerPos;//�÷��̾� ���� �ִ� ��ġ
    Vector2 battlePos;
    Vector2 middlePos;
    private float positionOffset=-7f;
    private bool isMoveDone=false; // battlePos���� �̵��� ���ƴ���?

    public Vector2 BattlePos { get => battlePos; }
    #endregion

    #region �ܺ�Ŭ����
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
        Debug.Log("�÷��̾� �� ����");
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
        Debug.Log("�÷��̾� �� ��");
        yield return null;
    }
    void ActionSwitch(int actionCase)
    { //�÷��̾� �Ͽ��� �� ����Ŭ�� �ൿ(���������� �������԰� Ȥ�ó� �� �Ͽ� �ൿ�� �ι� �ϰ� �� ���� ���� �� ���Ƽ� ����ġ�� ���)

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

    

    #region �̵� �Լ�
    public void DashToBattlePos() {
        SetBattlePos();
        Debug.Log(battlePos);
        Debug.Log("DashToEnemy ����");
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
        transform.DOMove(new Vector3(battlePos.x + positionOffset, battlePos.y), (float)stageManager.SecondsPerBeat).SetEase(Ease.OutQuart); //ActionStartTurn���� ȣ��� ���� actionCase������Ű�°� ������..OnComplete(() => ActionSwitch(++actionCase)
        Debug.Log("�̵��Ϸ��� battlePos : "+battlePos);
        isMoveDone = true;
    }


    public void ReturnToOriginPos() {
        playerAnimator.BackDash();
        transform.DOMove(originPlayerPos, dashDuringTime).SetEase(Ease.OutQuart).OnComplete(()=>playerAnimator.Idle());
                
    }
    #endregion

    void DecreaseOnBeat()
    {//OnBeating�� �����ؼ� ����ϴ� ī��Ʈ �ٿ�
        delayBeatCount--;
    }
    void AttackCountDown() {
        playerAnimator.attackCountDown();
        uiSound.SignalSound(gameObject, false); //���� ���� ��ȣ
            }

    IEnumerator WaitForAnAttack()
    { //������ �����ϰ� �����ϱ� ������ ��ٸ���
        Debug.Log("WaitForAnAttacking ����");
        delayBeatCount = waitForAttackDelay;
        Metronome.instance.OnBeating += DecreaseOnBeat; //��Ʈ���� ȣ��Ǵ� OnBeating�� �������Ѽ� ��Ʈ���� delayBeatCount����
        Metronome.instance.OnBeating += AttackCountDown;
        while (true)
        {
            if (delayBeatCount == 1) {
                Metronome.instance.OnBeating -= AttackCountDown; //���ݽ�ȣ�� ��ȣ ���� 
            }
            if (delayBeatCount == 0)
            { // delayBeatCount��ŭ�� ���� �ڿ� ���ݽ�ȣ
                Metronome.instance.OnBeating -= DecreaseOnBeat;// ��������
                //StartCoroutine(AttackSignal());//���ݽ�ȣ ��ƼŬ���
                playerAnimator.attackSignalPlay();
                uiSound.SignalSound(gameObject, true); //���� ���� ��ȣ
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
        Debug.Log("ConsiderAttacking ����");
        delayBeatCount = noAttackDelay;// 4���� ���� �÷��̾� �� ����
        Debug.Log("delayBeatCount : "+delayBeatCount);
        Metronome.instance.OnBeating += DecreaseOnBeat;
        onBasicAttack?.Invoke(true);
        while (true)//�÷��̾� �� ���ѽð� 4����
        {

            //�б��� - �������� �� ���ѽð� ���� 
            if (Input.GetKeyDown(KeyCode.F))
            {
                onBasicAttack?.Invoke(false);
                Metronome.instance.OnBeating -= DecreaseOnBeat;//���� ���ϰ� ����ϴ� ������ ���ѽð��� �����
                yield return StartCoroutine(playerAttack.StartAttack());
                
                break;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                onBasicAttack?.Invoke(false);
                Metronome.instance.OnBeating -= DecreaseOnBeat;//���� ���ϰ� ����ϴ� ������ ���ѽð��� �����
                yield return StartCoroutine(playerAttack.StartAttack());
                break;
            }
            if (delayBeatCount == 0)//���ѽð� ����
            {
                Metronome.instance.OnBeating -= DecreaseOnBeat;//delayBeatCount�� 0�� �ż� �������
                Debug.Log("���ѽð� ����");
                //transform.DOMove(originPlayerPos, dashDuringTime).SetEase(Ease.OutQuart).OnComplete(PlayerTurnEnd);//ReturnToOriginPos�� ���յ�
                playerAnimator.Guard();
                PlayerTurnEnd();
                break;

            }
            yield return null;
        }
        //���� �� �� ����

        playerAnimator.Guard();
        PlayerTurnEnd();
        yield break;

    }


    IEnumerator AttackSignal()
    {
        Debug.Log("���ݽ�ȣ");   //�÷��̾ �����ϱ� �� ���ڿ� �˷��ִ� ��ȣ EnemyTurn�� ���ݽ�ȣ�� ���� ����

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