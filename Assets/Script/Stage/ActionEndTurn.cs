using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;


public class ActionEndTurn : ITurnHandler
{
    // Start is called before the first frame update
    StageManager stageManager;
    
    public void InitSettings(StageManager stageManager)
    {
        //this.prepareTurnUI = prepareTurnUI;
        this.stageManager = stageManager;
        

    }
    // Start is called before the first frame update
    public IEnumerator TurnStart()
    {
        Debug.Log("ActionEndTurn");
        
        stageManager.PlayerInterface.PlayerTurn.ReturnToOriginPos();
        stageManager.Enemy.ReturnToOriginPos();
        //battleCamManager.ZoomOut();
        //stageManager.Enemy.
        //stageManager.Enemy.
        //stageManager.Entity.

        yield return null;
    }

    // Update is called once per frame
    public IEnumerator TurnEnd()
    {
        stageManager.CurrentTurn = CustomEnum.Turn.PrepareTurn;
        yield return null;
    }
}
