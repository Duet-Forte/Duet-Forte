using System.Collections.Generic;
using UnityEngine;

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
        foreach (Quest quest in DataBase.Quest.Quests.Keys)
        {
            if (quest.ID == id)
                GrantReward(quest);
        }
        CheckPrecedingQuest();

    }
    public void SetQuest(int id)
    {
        Debug.Log("퀘스트 부여함!");
        DataBase.Quest.SetQuest(quests[id]);
    }
    public void GrantReward(Quest quest)
    {
        Debug.Log("보상 받음!");
        Debug.Log(quest.gold);
        Debug.Log(quest.experiencePoint);
        Debug.Log(quest.skillId);
        DataBase.Quest.CompleteQuest(quest);
        DataBase.Player.GetReward(quest);
        if(quest.interactor != null)
        {
            DataBase.Dialogue.SetID(quest.interactor, quest.dialogueIndex);
        }

    }
    public void CheckEliminationQuest(string name)
    {
        var questsCopy = new List<Quest>(DataBase.Quest.Quests.Keys);
        foreach (Quest quest in questsCopy)
        {
            if (quest is EliminationQuest && (quest as EliminationQuest).monsterName == name)
                GrantReward(quest);
        }
        CheckPrecedingQuest();
    }

    private void CheckPrecedingQuest()
    {
        var questsCopy = new List<Quest>(DataBase.Quest.Quests.Keys);
        foreach (Quest quest in questsCopy)
        {
            if (!(quest is PrecedingQuest))
                continue;
            PrecedingQuest temp = quest as PrecedingQuest;
            for (int i = 0; i < temp.questID.Length; ++i)
            {
                if (!DataBase.Quest.Quests[quests[temp.questID[i]]])
                {
                    return;
                }
            }
            GrantReward(quest);
        }
    }
    public Quest GetQuest(int id) 
    {
        return quests[id];
    }
}
