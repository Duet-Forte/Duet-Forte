using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerAttackTimingCheck : MonoBehaviour
{
    StageManager theStageManager;
    #region 판정관련 변수
    private double badJudgeTime;
    private double goodJudgeTime;
    private double greatJudgeTime;
    private double perfectJudgeTime;
    private bool isMissedInCurrentFrame; 
    private int earlyCount;
    #endregion
    void Start()
    {
        theStageManager = FindObjectOfType<StageManager>();
        badJudgeTime = Const.BAD_JUDGE * theStageManager.SecondsPerBeat;
        goodJudgeTime = Const.GOOD_JUDGE * theStageManager.SecondsPerBeat;
        greatJudgeTime=Const.GREAT_JUDGE *theStageManager.SecondsPerBeat;
        perfectJudgeTime=Const.PERFECT_JUDGE * theStageManager.SecondsPerBeat;
        
    }
    public CustomEnum.JudgeName CheckTiming() {
        double currentTiming = Metronome.instance.CurrentTime;
        if (currentTiming > badJudgeTime)
        {
            if (currentTiming >= theStageManager.SecondsPerBeat - perfectJudgeTime)
            {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Perfect}");
                return CustomEnum.JudgeName.Perfect;
            }

            if (currentTiming >= theStageManager.SecondsPerBeat - greatJudgeTime)
            {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Great}");
                return CustomEnum.JudgeName.Great;

            }

            if (currentTiming >= theStageManager.SecondsPerBeat - goodJudgeTime)
            {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Good}");
                return CustomEnum.JudgeName.Good;
            }
            else {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Bad}");
                return CustomEnum.JudgeName.Bad; }
           
            
            

        }
        else if (currentTiming <= badJudgeTime) {
            if (currentTiming <= perfectJudgeTime)
            { 
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Perfect}");
                return CustomEnum.JudgeName.Perfect;
               
            }

            if (currentTiming <= greatJudgeTime)
            {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Great}");
                return CustomEnum.JudgeName.Great;
                
            }

            if (currentTiming <= goodJudgeTime)
            {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Good}");
                return CustomEnum.JudgeName.Good;
               
            }
            else {
                Debug.Log($"플레이어 공격 : {CustomEnum.JudgeName.Bad}");
                return CustomEnum.JudgeName.Bad; 
            }

        }
        return CustomEnum.JudgeName.Perfect;//currentTime이 미세하게 secondsPerBeat를 넘은 경우
        

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
