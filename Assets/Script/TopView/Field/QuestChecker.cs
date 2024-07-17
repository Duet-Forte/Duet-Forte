using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChecker : EventTrigger
{
    [SerializeField] private string questTarget;
    //����Ʈ �޼� ��, �ش� ID�� ��ȭ ID�� ������
    [SerializeField] private int targetID;
    //�ش� ������ �޼��� ����Ʈ ID
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
