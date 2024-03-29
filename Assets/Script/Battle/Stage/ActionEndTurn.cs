using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;


public class ActionEndTurn : ITurnHandler
{
    // Start is called before the first frame update
    StageManager stageManager;
    GameObject blackBox;
    public void InitSettings(StageManager stageManager)
    {
        //this.prepareTurnUI = prepareTurnUI;
        this.stageManager = stageManager;
        blackBox = stageManager.BlackBox;

    }
    // Start is called before the first frame update
    public IEnumerator TurnStart()
    {
        Debug.Log("ActionEndTurn");
        blackBox.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        stageManager.PlayerInterface.PlayerTurn.ReturnToOriginPos();
        stageManager.Enemy.ReturnToOriginPos();

        yield return null;
    }

    // Update is called once per frame
    public IEnumerator TurnEnd()
    {
        stageManager.CurrentTurn = CustomEnum.Turn.PrepareTurn;
        yield return null;
    }
}
