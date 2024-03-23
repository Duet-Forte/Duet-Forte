using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;


public class Enemy : MonoBehaviour
{
    private StageManager stageManager;
    private EnemyData data;
    private int healthPoint;
    private QTE qteEffect;

    private float attackDelay;
    private float timeOffset;
    private int currentNoteIndex;

    private List<float> targetTimes;
    private List<bool> isNoteChecked;

    private float judgeStartTime;
    private float judgeEndTime;
    private int patternLength;
    private bool isEnteringGuardCounterPhase;

    private Transform playerTransform;
    private Vector2 originalPosition;
    private float positionOffset = 2f;

    public event Action OnFramePass;
    public event Action OnGuardCounterEnd;
    public event Action<int> OnAttack;
    public event Action<int> OnGetDamage;

    private PlayerAnimator playerAnim;//체인지 세트 72 - 플레이어에 접근해서 가드나 피격 애니메이션 재생시키기 위한 변수

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
    public float JudgeStartTime { get { return judgeStartTime; } }
    public float JudgeEndTime { get { return judgeEndTime; } }
    public EnemyData Data { get { return data; } }
    public Transform Transform { get { return transform; } }
    #endregion

    #region 디버깅용!
    public SpriteRenderer[] tri;
    #endregion

    public IEnumerator DisplayPattern(Pattern pattern)//패턴신호 + 공격위치로 이동
    {
        ResetSettings();
        patternLength = pattern.Signature.Length;
        AkSoundEngine.PostEvent("Signal", gameObject);
        for (int i = 0; i < patternLength; ++i)
        {
            float targetTime = (1f / pattern.Signature[i] * Const.QUARTER_NOTE) * stageManager.SecondsPerBeat;
            targetTimes.Add(targetTime);
            isNoteChecked.Add(true);
            yield return WaitForTargetedTime(targetTimes[i]);
            AkSoundEngine.PostEvent("Attack_Sound", gameObject);
            tri[i].gameObject.SetActive(true); //디버깅
        }
        Debug.Log($"{attackDelay}초 이후 시작");

        Vector2 targetPoint = new Vector2(playerTransform.position.x + positionOffset, playerTransform.position.y);
        transform.DOMove(targetPoint, attackDelay); //공격위치로 이동
        yield return WaitForTargetedTime(attackDelay);
    }

    public IEnumerator Attack()
    {
        playerAnim.Guard();//체인지 세트 72

        float patternStartTime = Time.time;
        float sumOfTime = 0;
        AkSoundEngine.PostEvent("Signal", gameObject);
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
                    break;
                yield return null;
            }
            if (!isNoteChecked[i])
            {
                GiveDamage();
                Debug.Log($"{judgeEndTime}에 판정 종료");
            }
        }
        while (judgeEndTime >= Time.time)
        {
            yield return null; // 끝나는 시간을 항상 같게 하기 위해.
        }

        if (isEnteringGuardCounterPhase)
        {
            yield return EnterGuardCounterPhase();
        }
        //playerAnim.Idle();//체인지 세트 72 - prepare턴으로 이사갈 예정~
        transform.DOMove(originalPosition, attackDelay);
        yield return WaitForTargetedTime(attackDelay);

        #region 디버깅!!!
        for (int i = 0; i < 4; ++i)
        {
            tri[i].gameObject.SetActive(false);
            tri[i].color = Color.white;
        }
        #endregion
    }

    private IEnumerator WaitForTargetedTime(float targetTime)// 메트로놈 OnBeat구독하면 필요없어질 듯
    {
        float elapsedTime = timeOffset;

        while (targetTime >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timeOffset = elapsedTime - targetTime;
    }

    public void BindData(EnemyData enemyData) => data = (enemyData.Equals(null)) ? data : enemyData;//데이터 주입, 프리팹으로 관리하면 필요없어질 듯
    public void InitSettings(StageManager currentStageManager, Transform playerTransform) // StageManager에서 호출되는 초기세팅
    {
        stageManager = currentStageManager;
        float sPB = stageManager.SecondsPerBeat;
        attackDelay = Const.ATTACK_DELAY_BEATS * sPB;
        targetTimes = new List<float>();
        isNoteChecked = new List<bool>();
        healthPoint = data.HP;
        this.playerTransform = playerTransform; //플레이어 위치
        #region 에너미 턴에 플레이어 자세
        playerAnim = playerTransform.gameObject.GetComponent<PlayerAnimator>();//체인지 세트 72
        #endregion
        originalPosition = transform.position; //원래 위치
        qteEffect = Instantiate(Resources.Load<GameObject>("Effect/QTE_Effect"))
            .GetComponent<QTE>();
        qteEffect.InitSettings(sPB);
        #region 디버깅!!!
        for (int i = 0; i < 4; ++i)
        {
            tri[i].gameObject.SetActive(false);   //공격신호 비활성으로 초기화
        }
        #endregion
    }

    private void ResetSettings()
    {
        targetTimes.Clear();
        isNoteChecked.Clear();
        currentNoteIndex = 0;
        timeOffset = 0;
    }

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
        OnGuardCounterEnd?.Invoke();
    }
    private IEnumerator ProgressGuardCounterPhase()
    {
        Vector2 targetPosition = (playerTransform.position + transform.position) / 2;
        qteEffect.StartQTE(targetPosition);
        yield return WaitForTargetedTime(qteEffect.Speed);
    }

    public void GiveDamage()
    {
        int damage = 1; // 임시적으로 1데미지 줌.
        Judge judge = new Judge();
        judge.Name = CustomEnum.JudgeName.Miss;
        HandleParryJudge(judge);
        OnAttack?.Invoke(damage);
    }

    private void GetDamage(int damage)  //퍼블릭으로 돌리고 플레이어가 접근하면 됨
    {
        healthPoint -= damage;
        OnGetDamage?.Invoke(damage);
        if (healthPoint <= 0)
            stageManager.OnEnemyDie();
    }
    public void HandleParryJudge(Judge judge, int damage = 0)
    {
        if(judge.Name.Equals(CustomEnum.JudgeName.Perfect))
            GetDamage(damage);                                      //임의로 패링 퍼펙트 했을 때 대미지 받음
        if (judge.Name.Equals(CustomEnum.JudgeName.Miss))
        {
            AkSoundEngine.PostEvent("Miss", gameObject);
            playerAnim.Hurt(); //체인지 세트 72
        }
        else
        {
            AkSoundEngine.PostEvent("Parry_Sound", gameObject);
            //playerAnim.Parry();
            //체인지 세트 72
        }
        tri[currentNoteIndex].color = judge.Color;
        isNoteChecked[currentNoteIndex] = true;
        ++currentNoteIndex;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void StopActions() => StopAllCoroutines();
}
