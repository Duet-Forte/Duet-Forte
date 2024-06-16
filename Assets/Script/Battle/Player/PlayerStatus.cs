using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerStatus : MonoBehaviour
{
    /*
     * 레벨
     * 체력
     * 가드카운터 게이지
     * 경험치
     * 공격력
     * 방어력
     * 
    */

    //일단은 임시로 직렬화
    private int playerLevel;
    [SerializeField] private int playerHealthPoint;
    private int playerGuardCounterGuage;
    private int playerCurrentExp;
    private int playerMaxExp;
    [SerializeField] private float playerAttack;//공격력
    [SerializeField] private float playerDefence;//방어력


    public int PlayerHealthPoint { get => playerHealthPoint; 
        set { 
            playerHealthPoint -= value; 
            if (playerHealthPoint <= 0) {
                playerHealthPoint = 0; 

            } 
        }
    }

    
    public int PlayerLevel { get => playerLevel; set { playerLevel = value; } }
    public float PlayerAttack { get => playerAttack; set {  playerAttack= value; } }
    public float PlayerDefence { get => playerDefence; set { playerDefence = value; } }
    public int PlayerGuardCounterGuage { get => playerGuardCounterGuage;}

    public int PlayerMaxEXP
    {
        get {
            return playerMaxExp;
        }

         }
    public int PlayerCurrentExp
    {
        get {
            return playerCurrentExp;
        }
    }
    

    public void InitSetting(int level,int currentExp) { //비전투 씬에서 받아온 레벨과 아이템을 기반으로 전투에 필요한 스탯을 재설정하는 함수
        playerGuardCounterGuage = Util.Const.GUARD_COUNTER_GAUGE;
        playerLevel = level;
        Debug.Log($"playerLevel in Status : {playerLevel}");
        playerCurrentExp= currentExp;
        SetMaxExp(playerLevel);
        
    }
    private void SetMaxExp(int level) {
        playerMaxExp = new Calculator().CalcMaxExp(level); //누적경험치는 현재레벨^3 공식임 즉, 현재 레벨의 필요경험치는 (다음 레벨^3)-(현재 레벨^3)


        if (level == 1) {
            playerMaxExp = (level + 1) * (level + 1) * (level + 1); // 1레벨에서  2레벨 가는 필요 경험치 2^3 
            }
    }
    
}
