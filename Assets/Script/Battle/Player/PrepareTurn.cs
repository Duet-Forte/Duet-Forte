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
    private Tutorial tutorial;
    public void InitSettings(StageManager stageManager)
    {
       
        this.stageManager = stageManager;
        prepareTurnUI = stageManager.PrepareTurnUI;
        turnUI = stageManager.TurnUI;
        UIDesolveTime = 0.8f;
        tutorial = stageManager.TutorialUI;
        
    }
    
    public IEnumerator TurnStart()
    {
        Debug.Log("PrepareTurn");
        
        turnUI.AppearanceTurnUI(stageManager.TurnCount);
        Debug.Log(prepareTurnUI.AppearSkillUI());
        prepareTurnUI.UISwitch(true);
        //���ӿ��� �� ���� ����
        yield return Timer(3f);
        if (tutorial != null)
        {
            yield return tutorial.InitSettings();
        }
        yield return null;
    }

    public IEnumerator TurnEnd()
    {
        yield return Timer(7);
        //StartCoroutine(prepareTurnUI.PopDownSkillUI());
        //��ü �� ���� �ڵ�� �ȵǰ� �Ʒ� �ڵ�� �Ǵ°���
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
            if (Input.GetKeyDown(KeyCode.Space)) elapsedTime = time;
            yield return null;
        }
    }
}
