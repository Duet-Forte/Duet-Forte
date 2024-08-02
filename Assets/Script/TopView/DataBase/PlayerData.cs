using System.Collections.Generic;

// 저장할 데이터들
public class PlayerData
{
    private int hp;
    private int level;
    private int experiencePoint;
    private int gold;

    public int Level { get => level; }
    public int EXP { get => experiencePoint; }
    public int Gold { get => gold; }
    
    public PlayerData()
    {
        hp = 1;
        level = 1;
        experiencePoint = 0;
        gold = 0;
    }
    public void GetReward(Quest quest)
    {
        DataBase.Skill.ActivateSkill(quest.skillId);
        gold += quest.gold;
        experiencePoint += quest.experiencePoint;
    }
    public void SetPlayerInfo(PlayerInfo info)
    {
        hp = info.PlayerHP;
        experiencePoint = info.PlayerCurrentEXP;
        level = info.PlayerLevel;
    }
    public PlayerInfo CreatePlayerInfo()
    {
        return new PlayerInfo(DataBase.Skill.Skill, level, experiencePoint);
    }

    public void LoadData(int level, int exp, int gold)
    {
        this.level = level;
        this.experiencePoint = exp;
        this.gold = gold;
    }
}
