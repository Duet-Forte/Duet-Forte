using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataBase : MonoBehaviour
{
    private Dictionary<Quest, bool> quests;
    public Dictionary<Quest, bool> Quests { get { if (quests == null) quests = new Dictionary<Quest, bool>(); return quests; } }

    public void SetQuest(Quest quest)
    {
        if(!Quests.TryGetValue(quest, out bool isDone))
            Quests.Add(quest, false);
    }

    public void CompleteQuest(Quest quest)
    {
        if(Quests.TryGetValue(quest, out bool isDone) && isDone == false)
        {
            DataBase.Player.GetReward(quest);
            Quests[quest] = true;
        }
    }

    public void LoadData(Dictionary<int, bool> questSaveData)
    {
        Dictionary<Quest, bool> quests = new Dictionary<Quest, bool>();
        foreach(var questData in questSaveData)
        {
            quests.Add(QuestManager.Instance.GetQuest(questData.Key), questData.Value);
        }
        this.quests = quests;
    }
}
