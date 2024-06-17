using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    int playerLevel;
    int playerCurrentEXP;
    PlayerSkill[] playerSkills;
    public PlayerInfo(int playerLevel, int playerCurrentEXP) {
        this.playerLevel = playerLevel;
        this.playerCurrentEXP = playerCurrentEXP;
    }
    public PlayerInfo(PlayerSkill[] playerSkills, int playerLevel, int playerCurrentEXP)
    {
        this.playerSkills = playerSkills;
        this.playerCurrentEXP = playerCurrentEXP;
        this.playerLevel = playerLevel;

    }

    public int PlayerLevel {
        get { return playerLevel; }
    }
    public int PlayerCurrentEXP {
        get { return playerCurrentEXP; }
    }
    public PlayerSkill[] PlayerSkills{
        get { return playerSkills; }
    }
}
