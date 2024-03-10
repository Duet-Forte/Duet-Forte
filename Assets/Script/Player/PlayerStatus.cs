using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private int playerLevel;
    [SerializeField] private int playerHealthPoint;
    [SerializeField] private int playerGuardCounterGuage;
    [SerializeField] private int playerExp;
    [SerializeField] private int maxPlayerExp;
    [SerializeField] private float playerAttack;//공격력
    [SerializeField] private float playerDefence;//방어력

    
    public int PlayerHealthPoint { get => playerHealthPoint; }
    public float PlayerAttack { get => playerAttack; }
    public float PlayerDefence { get => playerDefence; }
    public int PlayerGuardCounterGuage { get => playerGuardCounterGuage; }

    public int GetEXP { 
        set { playerExp = playerExp + value;
        if (playerExp >= maxPlayerExp) {
                playerLevel++;
                playerExp = maxPlayerExp - playerExp;
            } } }
    

    public void InitSetting() { //비전투 씬에서 받아온 레벨과 아이템을 기반으로 전투에 필요한 스탯을 재설정하는 함수
    
    
    }
    
}
