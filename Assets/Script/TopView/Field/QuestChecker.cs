using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChecker : EventTrigger
{
    [SerializeField] private string questTarget;
    //퀘스트 달성 시, 해당 ID로 대화 ID를 변경함
    [SerializeField] private int targetID;
    //해당 지역에 달성한 퀘스트 ID
    [SerializeField] private Quest quest;
    protected override void RunTask()
    {
        if (DataBase.Player.Quests.Contains(quest))
        {
            DataBase.Dialogue.SetID(questTarget, targetID);
            QuestManager.Instance.GrantReward(quest);
        }
    }
}
