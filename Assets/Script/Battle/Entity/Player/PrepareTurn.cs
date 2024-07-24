using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Util;
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
        //stageManager.Enemy.ReturnToOriginPos();
        turnUI.AppearanceTurnUI(stageManager.TurnCount);
        prepareTurnUI.UISwitch(true);
        //���ӿ��� �� ���� ����
        yield return new WaitForSeconds(3f);
        if (tutorial != null)
        {
            
            if (PlayerPrefs.GetInt(Const.IS_TUTORIAL_END) == 0)
            {
                yield return new WaitForSeconds(2.7f);
                yield return tutorial.InitSettings();
                PlayerPrefs.SetInt(Const.IS_TUTORIAL_END, 1);
            }
        }
        
    }

    public IEnumerator TurnEnd()
    {
        yield return new WaitForSeconds(0.3f);
        yield return Timer(7);
        //StartCoroutine(prepareTurnUI.PopDownSkillUI());
        //��ü �� ���� �ڵ�� �ȵǰ� �Ʒ� �ڵ�� �Ǵ°���
        prepareTurnUI.UISwitch(false);
        yield return new WaitForSeconds(UIDesolveTime);
        stageManager.CurrentTurn = Turn.ActionStartTurn;
        
    }

    IEnumerator Timer(float time)
    {
        float elapsedTime = 0;
        while (elapsedTime <= time)
        {
            elapsedTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space)) { 
                elapsedTime = time;
                break;
            }
            yield return null;
        }
    }
}
