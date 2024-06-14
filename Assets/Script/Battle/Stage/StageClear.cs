using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.CustomEnum;
using TMPro;
using UnityEngine.UI;
public class StageClear : MonoBehaviour
{
    // 판정과 총 공격 추가
    // 계산 시스템

    #region judgePoint
    private int perfectPoint = 10;
    private int greatPoint = 8;
    private int goodPoint = 3;
    private int missPoint = 0;
    private Rank rank;
    #endregion

    private static StageClear instance=null;
    private List<JudgeName> judges;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Slider expSlider;
    
    #region public Interface
   public void InitSettins() {
        judges = new List<JudgeName>();

        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
    public static StageClear Instance
    {
        get 
        {
            if (Instance == null) 
            {
                return null;
            }
            return Instance;
        }
    }
    public void AddJudge(JudgeName judge) { 
            judges.Add(judge);
        }
    public void CalcRank(int turn) {
        // 마지막으로 턴수를 입력받으며 최종 랭크 책정
        rank =Rank.Empty;
        string rankStr="";
        int resultPoint=judgeToPoint() + TurnToPoint(turn);

        if (resultPoint >= 19) {
            rank = Rank.S;
        }
        if (resultPoint >= 17) {
            rank = Rank.APlus;
        }
        if (resultPoint >= 15) {
            rank = Rank.A;
        }
        if (resultPoint >= 12) {
            rank=Rank.BPlus;
        }
        if (resultPoint >= 8) {
            rank = Rank.B;
        }
        if (resultPoint < 8) {
            rank = Rank.C;
        }

        switch (rank) { 
            case Rank.S: rankStr= "S"; break;
            case Rank.APlus: rankStr = "A+"; break;
            case Rank.A: rankStr = "A"; break;
            case Rank.BPlus: rankStr = "B+"; break;
            case Rank.B: rankStr = "B"; break;
            case Rank.C: rankStr = "C"; break;
            default: rankStr = "Empty"; break;
        }

    }
    #endregion

    #region 구현부
    private int judgeToPoint() {
        float sumPoint=0;
        for (int judgesIndex = 0; judgesIndex < judges.Count; judgesIndex++) {
            switch (judges[judgesIndex]) {
                case JudgeName.Perfect:
                    sumPoint += perfectPoint;
                    break;
                case JudgeName.Great:
                    sumPoint += greatPoint;
                    break;
                case JudgeName.Good:
                    sumPoint += goodPoint;
                    break;
                case JudgeName.Miss:
                    sumPoint += missPoint;
                    break;
                default:break;
            }
        }
        float  meanPoint= sumPoint /judges.Count;
        return MeanToPoint(meanPoint);

    }
    private int MeanToPoint(float mean) {

        if (mean >= 9.5f) {
            return 10;
        }
        if (mean >= 9.0f) {
            return 9;
        }
        if (mean >= 8.5f) {
            return 8;
        }
        if(mean >=8.0f){
            return 7;
        }
        if (mean >= 7.0) {
            return 5;
        }
        if (mean >= 5.5) {
            return 3;
        }
        return 1;
    }

    private int TurnToPoint(int turn) {

        switch (turn) {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6: return 10;
            case 7: return 9;
            case 8: return 8;
            case 9: return 7;
            case 10: return 5;
            case 11: return 3;
            default: return 1;


        }
    
    }
    #endregion
    // 판정 추가하는 시스템
    // 판정 별로 점수 내기
    // 판정 점수를 평균내기
    // 평균낸 점수의 분위구간 확인
    // 턴수 체크 분위구간 확인 후 점수 

    // 점수 분위구간 확인
    // Update is called once per frame
  
}
