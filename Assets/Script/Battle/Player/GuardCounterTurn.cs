using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GuardCounterTurn : ITurnHandler
{
    StageManager stageManager;
    PlayerGuardCounter playerGuardCounter;
    public void InitSettings(StageManager stageManager)
    {
        this.stageManager = stageManager;
        playerGuardCounter = stageManager.PlayerInterface.PlayerGuardCounter;
        

    }
    public IEnumerator TurnStart()
    {
        //�÷��̾��� ����ī���� ������ Ȯ��
        
        yield return playerGuardCounter.EnterGuardCounterPhase();
        //����ī���� �������� Ǯ�����̸� ����ī���� ����
        //QTE Ȱ��ȭ
        Debug.Log("����ī���� �� ����");
        yield return null;
    }
    

    public IEnumerator TurnEnd()
    {
        //���� ���� ��ġ�� ���ư�.
        stageManager.CurrentTurn = CustomEnum.Turn.ActionEndTurn;
        Debug.Log("����ī���� �� ����");
        yield return null;
    }
}
