using System;
using System.Collections;
using UnityEngine;
using Util.CustomEnum;

public class PrepareTurn :ITurnHandler
{
    private StageManager stageManager;
    private GameObject prepareTurnUIAsGameObject;
    private PrepareTurnUI prepareTurnUI;
    private float UIDesolveTime;
    ControlTurnUI turnUI;
    public void InitSettings(StageManager stageManager)
    {
        //this.prepareTurnUI = prepareTurnUI;
        this.stageManager = stageManager;
        prepareTurnUI = stageManager.PrepareTurnUI;
        turnUI = stageManager.TurnUI;
        UIDesolveTime = 0.8f;
        
    }
    //
    public IEnumerator TurnStart()
    {
        Debug.Log("PrepareTurn");
        //yield return new WaitUntil(() => prepareTurnUI.PopUpSkillUI()!=null);
        turnUI.AppearanceTurnUI(stageManager.TurnCount);
        Debug.Log(prepareTurnUI.AppearSkillUI());
        prepareTurnUI.UISwitch(true);
        yield return null;
    }

    public IEnumerator TurnEnd()
    {
        yield return Timer(7);
        //StartCoroutine(prepareTurnUI.PopDownSkillUI());
        //대체 왜 위에 코드는 안되고 아래 코드는 되는거지
        prepareTurnUI.UISwitch(false);
        yield return new WaitForSeconds(UIDesolveTime);
        Debug.Log("PrepareTurn End");
        stageManager.CurrentTurn = Turn.ActionStartTurn;
        
    }

    IEnumerator Timer(float time)
    {
        float elapsedTime = 0;
        while (elapsedTime <= time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
