using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ActionStartTurn : ITurnHandler
{
    //플레이어와 적을 맞짱뜨는 공간으로 데려오는 턴 순서(PrepareTurn -> ActionStartTurn -> PlayerTurn -> EnemyTurn -> ActionEndTurn -> PrepareTurn)
    // 카메라의 줌인도 담당
    //
    StageManager stageManager;
    
    
    
    public void InitSettings(StageManager stageManager)
    {
        
        this.stageManager = stageManager;
        

    }
    
    public IEnumerator TurnStart() {
        Debug.Log("ActionStartTurn");

        stageManager.SelectRandomBattlePos();
        //battleCamManager.ZoomIn();
        Debug.Log("재설정된 battlePos : "+stageManager.BattlePos);
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
