using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ActionStartTurn : ITurnHandler
{
    //�÷��̾�� ���� ��¯�ߴ� �������� �������� �� ����(PrepareTurn -> ActionStartTurn -> PlayerTurn -> EnemyTurn -> ActionEndTurn -> PrepareTurn)
    // ī�޶��� ���ε� ���
    //
    StageManager stageManager;
    GameObject blackBox;
    
    
    public void InitSettings(StageManager stageManager)
    {
        
        this.stageManager = stageManager;
        blackBox = stageManager.BlackBox;

    }
    
    public IEnumerator TurnStart() {
        Debug.Log("ActionStartTurn");
        blackBox.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.45f);
        stageManager.SelectRandomBattlePos();
        //battleCamManager.ZoomIn();
        Debug.Log("�缳���� battlePos : "+stageManager.BattlePos);
        stageManager.PlayerInterface.PlayerTurn.DashToBattlePos();
        stageManager.Enemy.DashToBattlePos();
        yield return new WaitUntil(() => stageManager.PlayerInterface.PlayerTurn.IsMoveDone);
        //battleCam = FindObjectOfType<Camera>();
        //battleCam.orthographicSize = 16;
        yield return null;
    }

    // Update is called once per frame
    public IEnumerator TurnEnd()
    {
        stageManager.CurrentTurn = CustomEnum.Turn.PlayerTurn;
        yield return null;
    }
}
