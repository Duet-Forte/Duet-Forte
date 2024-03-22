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

    private PlayerAnimator playerAnim;//ü���� ��Ʈ 72 - �÷��̾ �����ؼ� ���峪 �ǰ� �ִϸ��̼� �����Ű�� ���� ����

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
    public float JudgeStartTime { get { return judgeStartTime; } }
    public float JudgeEndTime { get { return judgeEndTime; } }
    public EnemyData Data { get { return data; } }
    public Transform Transform { get { return transform; } }
    #endregion

    #region ������!
    public SpriteRenderer[] tri;
    #endregion

    public IEnumerator DisplayPattern(Pattern pattern)//���Ͻ�ȣ + ������ġ�� �̵�
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
            tri[i].gameObject.SetActive(true); //�����
        }
        Debug.Log($"{attackDelay}�� ���� ����");

        Vector2 targetPoint = new Vector2(playerTransform.position.x + positionOffset, playerTransform.position.y);
        transform.DOMove(targetPoint, attackDelay); //������ġ�� �̵�
        yield return WaitForTargetedTime(attackDelay);
    }

    public IEnumerator Attack()
    {
        playerAnim.Guard();//ü���� ��Ʈ 72

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
                Debug.Log($"{judgeEndTime}�� ���� ����");
            }
        }
        while (judgeEndTime >= Time.time)
        {
            yield return null; // ������ �ð��� �׻� ���� �ϱ� ����.
        }

        if (isEnteringGuardCounterPhase)
        {
            yield return EnterGuardCounterPhase();
        }
        //playerAnim.Idle();//ü���� ��Ʈ 72 - prepare������ �̻簥 ����~
        transform.DOMove(originalPosition, attackDelay);
        yield return WaitForTargetedTime(attackDelay);

        #region �����!!!
        for (int i = 0; i < 4; ++i)
        {
            tri[i].gameObject.SetActive(false);
            tri[i].color = Color.white;
        }
        #endregion
    }

    private IEnumerator WaitForTargetedTime(float targetTime)// ��Ʈ�γ� OnBeat�����ϸ� �ʿ������ ��
    {
        float elapsedTime = timeOffset;

        while (targetTime >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timeOffset = elapsedTime - targetTime;
    }

    public void BindData(EnemyData enemyData) => data = (enemyData.Equals(null)) ? data : enemyData;//������ ����, ���������� �����ϸ� �ʿ������ ��
    public void InitSettings(StageManager currentStageManager, Transform playerTransform) // StageManager���� ȣ��Ǵ� �ʱ⼼��
    {
        stageManager = currentStageManager;
        float sPB = stageManager.SecondsPerBeat;
        attackDelay = Const.ATTACK_DELAY_BEATS * sPB;
        targetTimes = new List<float>();
        isNoteChecked = new List<bool>();
        healthPoint = data.HP;
        this.playerTransform = playerTransform; //�÷��̾� ��ġ
        #region ���ʹ� �Ͽ� �÷��̾� �ڼ�
        playerAnim = playerTransform.gameObject.GetComponent<PlayerAnimator>();//ü���� ��Ʈ 72
        #endregion
        originalPosition = transform.position; //���� ��ġ
        qteEffect = Instantiate(Resources.Load<GameObject>("Effect/QTE_Effect"))
            .GetComponent<QTE>();
        qteEffect.InitSettings(sPB);
        #region �����!!!
        for (int i = 0; i < 4; ++i)
        {
            tri[i].gameObject.SetActive(false);   //���ݽ�ȣ ��Ȱ������ �ʱ�ȭ
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
        Debug.Log("���� ī����!");
        yield return ProgressGuardCounterPhase(); //���� ī���� �� ����
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
        int damage = 1; // �ӽ������� 1������ ��.
        Judge judge = new Judge();
        judge.Name = CustomEnum.JudgeName.Miss;
        HandleParryJudge(judge);
        OnAttack?.Invoke(damage);
    }

    private void GetDamage(int damage)  //�ۺ����� ������ �÷��̾ �����ϸ� ��
    {
        healthPoint -= damage;
        OnGetDamage?.Invoke(damage);
        if (healthPoint <= 0)
            stageManager.OnEnemyDie();
    }
    public void HandleParryJudge(Judge judge, int damage = 0)
    {
        if(judge.Name.Equals(CustomEnum.JudgeName.Perfect))
            GetDamage(damage);                                      //���Ƿ� �и� ����Ʈ ���� �� ����� ����
        if (judge.Name.Equals(CustomEnum.JudgeName.Miss))
        {
            AkSoundEngine.PostEvent("Miss", gameObject);
            playerAnim.Hurt(); //ü���� ��Ʈ 72
        }
        else
        {
            AkSoundEngine.PostEvent("Parry_Sound", gameObject);
            //playerAnim.Parry();
            //ü���� ��Ʈ 72
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
