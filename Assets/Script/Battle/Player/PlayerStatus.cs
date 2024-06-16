using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerStatus : MonoBehaviour
{
    /*
     * ����
     * ü��
     * ����ī���� ������
     * ����ġ
     * ���ݷ�
     * ����
     * 
    */

    //�ϴ��� �ӽ÷� ����ȭ
    private int playerLevel;
    [SerializeField] private int playerHealthPoint;
    private int playerGuardCounterGuage;
    private int playerCurrentExp;
    private int playerMaxExp;
    [SerializeField] private float playerAttack;//���ݷ�
    [SerializeField] private float playerDefence;//����


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
    

    public void InitSetting(int level,int currentExp) { //������ ������ �޾ƿ� ������ �������� ������� ������ �ʿ��� ������ �缳���ϴ� �Լ�
        playerGuardCounterGuage = Util.Const.GUARD_COUNTER_GAUGE;
        playerLevel = level;
        Debug.Log($"playerLevel in Status : {playerLevel}");
        playerCurrentExp= currentExp;
        SetMaxExp(playerLevel);
        
    }
    private void SetMaxExp(int level) {
        playerMaxExp = new Calculator().CalcMaxExp(level); //��������ġ�� ���緹��^3 ������ ��, ���� ������ �ʿ����ġ�� (���� ����^3)-(���� ����^3)


        if (level == 1) {
            playerMaxExp = (level + 1) * (level + 1) * (level + 1); // 1��������  2���� ���� �ʿ� ����ġ 2^3 
            }
    }
    
}
