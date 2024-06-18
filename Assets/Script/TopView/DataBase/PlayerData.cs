using System.Collections.Generic;

public class PlayerData
{
    private int level;
    private int experiencePoint;
    private int gold;
    private List<Quest> quests;

    public List<Quest> Quests { get { if (quests == null) quests = new List<Quest>(); return quests; }}

    public PlayerData()
    {
        level = 1;
        experiencePoint = 0;
        gold = 0;
    }
    public void GetReward(Quest quest)
    {
        quests.Remove(quest);
        DataBase.Instance.Skill.ActivateSkill(quest.skillId);
        gold += quest.gold;
        experiencePoint += quest.experiencePoint;
    }
    public void SetPlayerInfo(PlayerInfo info)
    {
        experiencePoint =info.PlayerCurrentEXP;
        level = info.PlayerLevel;
    }
    public PlayerInfo CreatePlayerInfo()
    {
        return new PlayerInfo(DataBase.Instance.Skill.Skill, level, experiencePoint);
    }
}
