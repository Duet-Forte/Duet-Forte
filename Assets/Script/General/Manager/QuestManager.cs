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
    }
    public void SetQuest(int id)
    {
        Debug.Log("����Ʈ �ο���!");
        DataBase.Quest.SetQuest(quests[id]);
    }
    public void GrantReward(Quest quest)
    {
        Debug.Log("���� ����!");
        Debug.Log(quest.gold);
        Debug.Log(quest.experiencePoint);
        Debug.Log(quest.skillId);
        DataBase.Quest.CompleteQuest(quest);
    }

    public Quest GetQuest(int id) 
    {
        return quests[id];
    }
}
