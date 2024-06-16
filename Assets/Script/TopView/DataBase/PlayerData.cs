using System.Collections.Generic;

public class PlayerData
{
    private SkillSet[] equipedSkillSet;
    private int level;
    private int experiencePoint;
    private int gold;
    private List<Quest> quests;

    public SkillSet[] SkillSets { get { return equipedSkillSet;}}
    public List<Quest> Quests { get { if (quests == null) quests = new List<Quest>(); return quests; }}
    public void SetSkills(SkillSet skill)
    {
        
    }

    public void GetReward(Quest quest)
    {
        quests.Remove(quest);
        DataBase.Instance.Skill.ActivateSkill(quest.skillId);
        gold += quest.gold;
        experiencePoint += quest.experiencePoint;
    }
}
