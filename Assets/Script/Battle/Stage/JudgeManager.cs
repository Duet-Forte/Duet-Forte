using System;
using UnityEngine;
using Util;
using Util.CustomEnum;

public class JudgeManager
{
    private StageManager stageManager;
    private PlayerInterface playerInterface;
    private int healthPoint; // ���� ���ǻ� �ӽ÷� ���� ������, ���� Ŭ���� ���谡 �Ϸ�Ǹ� player�� stat�� �ٷ�� Ŭ�������� ������ ������ ����.
    private int maxGauge; // ��������
    private int combo;

    public event Action<Judge> OnParryEnd;
    public event Action<Judge> OnParrySuccess;
    public event Action<int, int> OnComboChange;//UI �������� �ݿ�
    #region ������Ƽ
    public int GuardGauge { get { return maxGauge; } set { maxGauge = value; } }
    public int EarlyCount { get { return earlyCount; } set { earlyCount = value; } }
    public int HP { get { return healthPoint; } }
    #endregion

    #region �����ð� ���� ����
    private float goodJudgeTime;
    private float greatJudgeTime;
    private float perfectJudgeTime;
    private bool isMissedInCurrentFrame; // �� �����ӿ� missNote ���Ŀ� checkscore�� ȣ��� ���, ������ perfect ������ ���� ���� ����. /���� ������ ������ ���� ���׸� �����ϱ� ���� ����
    private int earlyCount; // ��Ÿ ���� �� ������ �������� �Է½ÿ� ������ miss�� �Ҵ��ϴ� ����� ������ �ý����� ����� �Է� ����
    #endregion

    public void InitSettings(StageManager currentStageManager)
    {
        stageManager = currentStageManager;
        playerInterface = currentStageManager.PlayerInterface;
        healthPoint = playerInterface.PlayerStatus.PlayerHealthPoint;// �̻翹��
        maxGauge = currentStageManager.PlayerInterface.PlayerStatus.PlayerGuardCounterGuage; //��������
        goodJudgeTime = Const.GOOD_JUDGE * stageManager.SecondsPerBeat;
        greatJudgeTime = Const.GREAT_JUDGE * stageManager.SecondsPerBeat;
        perfectJudgeTime = Const.PERFECT_JUDGE * stageManager.SecondsPerBeat;
    }

    public void UpdateInput()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !stageManager.Enemy.IsNoteChecked)
        {
             Debug.Log($"�и� �õ�! {Time.time}�ʿ� �õ���");
             CheckScore(Time.time);//Space ������ ��, StartTime EndTime ������.
        }
    }

    public void IncreaseGauge(int combo) // �̵� ����
    {
        this.combo += combo;
        OnComboChange?.Invoke(this.combo, maxGauge);
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
                EndParry(new Judge(JudgeName.Miss));
                isMissedInCurrentFrame = true;
            }
            return;
        }
        else if (isMissedInCurrentFrame)
        {
            isMissedInCurrentFrame = false;
            return;
        }
        double judgeTime = Math.Abs(targetTime - parryTime);//���� ���� Ÿ�̹�
        Judge judge = new Judge();
        if (judgeTime <= perfectJudgeTime)
        {
            judge.Name = JudgeName.Perfect;
            IncreaseGauge(3);            
        }
        else if (judgeTime <= greatJudgeTime)
        {
            judge.Name = JudgeName.Great;
            IncreaseGauge(2);
           
        }
        else if (judgeTime <= goodJudgeTime)
        {
            judge.Name = JudgeName.Good;
            IncreaseGauge(1);
           
        }
        else
        {
            judge.Name = JudgeName.Bad;
        }
        
        Debug.Log(judgeTime);
        Debug.Log("���� ���� : " + judgeStartTime + " ���� �� : " + judgeEndTime);
        EndParry(judge);
    }
    private void EndParry(Judge judge)
    {
        Debug.Log($"Miss!");
        OnParryEnd?.Invoke(judge);
        
    }

    public void CheckMissFrame() => isMissedInCurrentFrame = false;
    public void ResetCombo()
    {
        combo = 0;
        OnComboChange?.Invoke(combo, maxGauge);
    }
}
