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
    [SerializeField] private int playerLevel;
    [SerializeField] private int playerHealthPoint;
    private int playerGuardCounterGuage;
    [SerializeField] private int playerExp;
    [SerializeField] private int maxPlayerExp;
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

    public float PlayerAttack { get => playerAttack; }
    public float PlayerDefence { get => playerDefence; }
    public int PlayerGuardCounterGuage { get => playerGuardCounterGuage; }

    public int GetEXP { 
        set { playerExp = playerExp + value;
        if (playerExp >= maxPlayerExp) {
                playerLevel++;
                playerExp = maxPlayerExp - playerExp;
            } } }
    

    public void InitSetting() { //������ ������ �޾ƿ� ������ �������� ������� ������ �ʿ��� ������ �缳���ϴ� �Լ�
        playerGuardCounterGuage = Util.Const.GUARD_COUNTER_GAUGE;
    
    }
    
}
