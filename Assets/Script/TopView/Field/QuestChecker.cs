using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChecker : EventTrigger
{
    [SerializeField] private string questTarget;
    [SerializeField] private int detectID;
    [SerializeField] private int targetID;
    protected override void RunTask()
    {
        if(detectID <= DataBase.Instance.Dialogue.GetID(questTarget))
            DataBase.Instance.Dialogue.SetID(questTarget, targetID);
    }
}
