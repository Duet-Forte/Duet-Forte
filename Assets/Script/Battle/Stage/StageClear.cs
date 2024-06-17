using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.CustomEnum;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Util;

public class StageClear : MonoBehaviour
{
    // ������ �� ���� �߰�
    // ��� �ý���

    #region judgePoint
    private int perfectPoint = 10;
    private int greatPoint = 8;
    private int goodPoint = 3;
    private int missPoint = 0;
    private Rank rank;
    #endregion

    public static StageClear instance=null;
    private List<JudgeName> judges;
    string rankStr = "";

    private int maxExp;
    private int currentExp;
    private int playerLevel;
    private PlayerStatus playerStatus;
    private float expGainSpeed=1.5f;
    private float fadeOutSpeed=2.5f;

    StageManager stageManager;
    [SerializeField] private GameObject rankTitle;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private GameObject pack;
    [SerializeField] private Image backGround;
    [SerializeField] private TMP_Text expChangeText;

   /* private void Start() //�׽�Ʈ �ڵ�
    {
        judges = new List<JudgeName>();
        AddJudge(JudgeName.Perfect);
        Invoke(3,true,50);
        //expSlider.value = ;
        //expSlider.value;
    }*/
    #region public Interface
    public void InitSettings(StageManager stageManager) {

        if (instance == null)
        {
            instance = this;
            
        }
        else {
            
        }
        
        stageManager = stageManager;
        judges = new List<JudgeName>();
        playerStatus = stageManager.PlayerInterface.PlayerStatus;
    }
    public static StageClear Instance
    {
        get 
        {
            if (instance == null) 
            {
                return null;
            }

            return instance;
        }
    }
    public void AddJudge(JudgeName judge) { 
            judges.Add(judge);
        }
    public void Invoke(int turn, bool isWin, int enemyExp) {
        
        pack.SetActive(true);
        CalcRank(turn);
        StartCoroutine(AppearCoroutine(isWin,enemyExp));
    }
 
    #endregion

    #region ������
   private void CalcRank(int turn) {
        // ���������� �ϼ��� �Է¹����� ���� ��ũ å��
        rank =Rank.Empty;
        
        int resultPoint=judgeToPoint() + TurnToPoint(turn);
        Debug.Log(resultPoint);
        if (resultPoint >= 8) {
            rank = Rank.B;
        }
        if (resultPoint >= 12) {
            rank=Rank.BPlus;
        }
        if (resultPoint >= 15) {
            rank = Rank.A;
        }
        if (resultPoint >= 17) {
            rank = Rank.APlus;
        }
        if (resultPoint >= 19) {
            rank = Rank.S;
        }
        if (resultPoint < 8) {
            rank = Rank.C;
        }
        Debug.Log("��ũ : "+rank);
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

    
    private IEnumerator AppearCoroutine(bool isWin, int enemyExp) {
        expGainSpeed = 1.5f;
        SetEXPSlider();
        expChangeText.text = $"+EXP {enemyExp}";
        //Coroutine showEXP= StartCoroutine(ShowEXP());
        if (isWin)
        {
            new SoundSet.BackGroundMusic().GameOver(isWin);

        }
        else { 
            //�й�� BGM
        }
        backGround.DOFade(0.7f, fadeOutSpeed);
        yield return new WaitForSeconds(fadeOutSpeed);

        
        titleText.gameObject.SetActive(true);
        if (isWin)
        {
            titleText.text = "Victory";
            
        }
        else {
            titleText.text = "Defeat";
        }
        titleText.transform.DOScale(new Vector2(1, 1), 0.2f);
        rankText.text = rankStr;

        if (isWin)
        {
            yield return new WaitForSeconds(0.7f);

            rankTitle.gameObject.SetActive(true);
            rankTitle.transform.DOScale(new Vector2(1, 1), 0.2f);
        }
        yield return new WaitForSeconds(0.7f);

        expSlider.transform.DOLocalMove(new Vector2(expSlider.transform.localPosition.x, -360), 0.4f).OnComplete(() => {
            ChangeEXP(enemyExp);
        });

    }

    private IEnumerator WaitForInput() {
        while (true) {
            if (Input.anyKeyDown) {
                Debug.Log("������������ �̵� �ǳ��� ������ : ����, ���� ����ġ");
            }
            yield return null;
        }
    }

    private void ChangeEXP(int enemyExp) {

        if (enemyExp == 0) return;
        int restEXP = maxExp - currentExp;
        Debug.Log($"enemyExp : {enemyExp}");
        Debug.Log($"restEXP : {restEXP}");
        Debug.Log($"maxEXP : {maxExp}");
        Debug.Log($"cerrentExp : {currentExp}");
        Debug.Log($"�������� �� ����ġ�ΰ�? : {enemyExp >= restEXP}");
        if (enemyExp >= restEXP) //�������ϴ� ��Ȳ
        {
            expGainSpeed = 0.7f + ((float)maxExp / enemyExp);//0.3�ʴ� ����ġ ������ �ּ� �ð�
            Tween changeExpText = DOTween.To(() => currentExp, value => expText.text = $"{value} / {maxExp}", maxExp, expGainSpeed);
            changeExpText.Play();
            Tween changeExp = DOTween.To(() => expSlider.value, value => expSlider.value = value, 1, expGainSpeed);
            changeExp.Play().OnComplete(() =>
            {
                LevelUP();
                ChangeEXP(enemyExp - restEXP);
            });

        }
        else if(enemyExp<restEXP) { //������ ���ϰ� ����ġ�� ä��� ��Ȳ
            int tmpExp = currentExp + enemyExp;
            expGainSpeed = 0.7f;
            Tween changeExpText = DOTween.To(() => currentExp, value => expText.text = $"{value} / {maxExp}", currentExp+enemyExp, expGainSpeed);//���� text ǥ��
            changeExpText.Play().OnComplete(() => { 
                currentExp += enemyExp;
                
            });
            Tween changeExp = DOTween.To(() => expSlider.value, value => expSlider.value = value, ((float)tmpExp / maxExp), expGainSpeed); //������ ������ ����
                changeExp.Play();
            
        
        }


    }

  
    private void SetEXPSlider() {
        
        playerLevel = playerStatus.PlayerLevel;
        maxExp = playerStatus.PlayerMaxEXP;
        currentExp = playerStatus.PlayerCurrentExp;
        levelText.text = $"Lv.{playerLevel}";
        expSlider.value = ((float)currentExp / maxExp);
        Debug.Log(
        $"playerLevel: {playerLevel}\nmaxEXP : {maxExp}\n currentExp : {currentExp} \n "
        );

    }
    private void TestSetExpSlide() {
        playerLevel = 3;
        maxExp = new Calculator().CalcMaxExp(playerLevel);
        currentExp = 10;
        expSlider.value = ((float)currentExp / maxExp);
        
    }
    private void LevelUP() {
        playerLevel++;
        levelText.text = $"Lv.{playerLevel}";
        expSlider.value = 0;
        maxExp = new Calculator().CalcMaxExp(playerLevel);
        currentExp = 0;
        expText.text = $"{currentExp} / {maxExp}";
        
    }

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
        if (judges.Count == 0) { return MeanToPoint(10); }
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
    // ���� �߰��ϴ� �ý���
    // ���� ���� ���� ����
    // ���� ������ ��ճ���
    // ��ճ� ������ �������� Ȯ��
    // �ϼ� üũ �������� Ȯ�� �� ���� 

    // ���� �������� Ȯ��
    // Update is called once per frame
  
}
