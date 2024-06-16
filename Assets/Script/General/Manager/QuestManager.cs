using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class QuestManager
{
    public QuestManager()
    {
        quests = Resources.LoadAll<Quest>("Scriptable/Quest");
    }

    private Quest[] quests;
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
                instance = new QuestManager();
            return instance;
        }
    }

    public void CheckQuest(int id)
    {
        foreach (Quest quest in DataBase.Instance.Player.Quests)
        {
            if (quest.ID == id)
                GrantReward(quest);
        }
    }
    public void SetQuest(int id)
    {
        Debug.Log("퀘스트 부여함!");
        DataBase.Instance.Player.Quests.Add(quests[id]);
    }
    public void GrantReward(Quest quest)
    {
        Debug.Log("보상 받음!");
        Debug.Log(quest.gold);
        Debug.Log(quest.experiencePoint);
        Debug.Log(quest.skillId);
        DataBase.Instance.Player.GetReward(quest);
    }

    public Quest GetQuest(int id) 
    {
        return quests[id];
    }
}
