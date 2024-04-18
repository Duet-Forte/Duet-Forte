using System;
using UnityEngine;
using Util;
using Util.CustomEnum;

public class JudgeManager
{
    private StageManager stageManager;
    private PlayerInterface playerInterface;
    private int healthPoint; // 구현 편의상 임시로 여기 있지만, 추후 클래스 설계가 완료되면 player의 stat을 다루는 클래스에서 참조를 가져올 예정.
    private int maxGauge; // 마찬가지
    private int combo;

    public event Action OnMissParry;
    public event Action<Judge, int> OnParrySuccess;
    public event Action<int, int> OnComboChange;//UI 게이지에 반영
    #region 프로퍼티
    public int GuardGauge { get { return maxGauge; } set { maxGauge = value; } }
    public int EarlyCount { get { return earlyCount; } set { earlyCount = value; } }
    public int HP { get { return healthPoint; } }
    #endregion

    #region 판정시간 관련 변수
    private float goodJudgeTime;
    private float greatJudgeTime;
    private float perfectJudgeTime;
    private bool isMissedInCurrentFrame; // 한 프레임에 missNote 이후에 checkscore가 호출될 경우, 무조건 perfect 판정이 나는 것을 방지. /다음 판정에 영향이 가는 버그를 예방하기 위한 변수
    private int earlyCount; // 연타 방지 한 판정에 연속적인 입력시에 판정을 miss로 할당하는 얼불춤 과부하 시스템의 사용자 입력 기준
    #endregion

    public void InitSettings(StageManager currentStageManager)
    {
        stageManager = currentStageManager;
        playerInterface = currentStageManager.PlayerInterface;
        healthPoint = playerInterface.PlayerStatus.PlayerHealthPoint;// 이사예정
        maxGauge = currentStageManager.PlayerInterface.PlayerStatus.PlayerGuardCounterGuage; //마찬가지
        goodJudgeTime = Const.GOOD_JUDGE * stageManager.SecondsPerBeat;
        greatJudgeTime = Const.GREAT_JUDGE * stageManager.SecondsPerBeat;
        perfectJudgeTime = Const.PERFECT_JUDGE * stageManager.SecondsPerBeat;
    }

    public void UpdateInput()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !stageManager.Enemy.IsNoteChecked)
        {
             Debug.Log($"패리 시도! {Time.time}초에 시도함");
             CheckScore(Time.time);//Space 눌렀을 때, StartTime EndTime 결정됨.
        }
    }

    public void IncreaseGauge() // 이동 예정
    {
        ++combo;
        OnComboChange?.Invoke(combo, maxGauge);
    }

    public void DecreaseHealthPoint(int damage) // 이동 예정 - BattlePresenter로
    {
        healthPoint -= damage;
        if (healthPoint <= 0)
            stageManager.OnPlayerDie();
    }

    private void CheckScore(float parryTime)
    {
        double judgeStartTime = stageManager.Enemy.JudgeStartTime;
        double judgeEndTime = stageManager.Enemy.JudgeEndTime;
        double targetTime = (judgeStartTime + judgeEndTime) / 2;

        if (parryTime < judgeStartTime)
        {
            Debug.Log("Early!");
            ++earlyCount;
            if (earlyCount >= Const.MAX_EARLY_COUNT)
            {
                MissNote();
            }
            return;
        }
        else if (isMissedInCurrentFrame)
        {
            isMissedInCurrentFrame = false;
            return;
        }
        double judgeTime = Math.Abs(targetTime - parryTime);//현재 판정 타이밍
        Judge judge = new Judge();
        if (judgeTime <= perfectJudgeTime)
        {
            judge.Name = JudgeName.Perfect;
            IncreaseGauge();
        }
        else if (judgeTime <= greatJudgeTime)
        {
            judge.Name = JudgeName.Great;
        }
        else if (judgeTime <= goodJudgeTime)
        {
            judge.Name = JudgeName.Good;
        }
        else
        {
            judge.Name = JudgeName.Bad;
        }
        int damage = UnityEngine.Random.Range(1,3);
        Debug.Log(judgeTime);
        Debug.Log("판정 시작 : " + judgeStartTime + " 판정 끝 : " + judgeEndTime);
        OnParrySuccess?.Invoke(judge, damage);
    }
    private void MissNote()
    {
        Debug.Log($"Miss!");
        OnMissParry?.Invoke();
        isMissedInCurrentFrame = true; 
    }

    public void CheckMissFrame() => isMissedInCurrentFrame = false;
    public void ResetCombo()
    {
        combo = 0;
        OnComboChange?.Invoke(combo, maxGauge);
    }
}
