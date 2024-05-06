using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.CustomEnum;
using DG.Tweening;

public class ActionStartTurn : ITurnHandler
{
    //플레이어와 적을 맞짱뜨는 공간으로 데려오는 턴 순서(PrepareTurn -> ActionStartTurn -> PlayerTurn -> EnemyTurn -> ActionEndTurn -> PrepareTurn)
    // 카메라의 줌인도 담당
    //
    StageManager stageManager;
    GameObject blackBox;
    
    
    public void InitSettings(StageManager stageManager)
    {
        
        this.stageManager = stageManager;
        blackBox = stageManager.BlackBox;

    }
    
    public IEnumerator TurnStart() 
    {
        Debug.Log("ActionStartTurn");
        blackBox.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.7f);
        stageManager.SelectRandomBattlePos();
        Debug.Log("재설정된 battlePos : "+stageManager.BattlePos);
        stageManager.PlayerInterface.PlayerTurn.DashToBattlePos();
        stageManager.Enemy.DashToBattlePos();
        yield return new WaitUntil(() => stageManager.PlayerInterface.PlayerTurn.IsMoveDone);
        yield return null;
    }

    // Update is called once per frame
    public IEnumerator TurnEnd()
    {
        stageManager.CurrentTurn = Turn.PlayerTurn;
        yield return null;
    }
}
