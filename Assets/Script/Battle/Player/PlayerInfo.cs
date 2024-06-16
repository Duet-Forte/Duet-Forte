using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    int playerLevel;
    int playerCurrentEXP;
    public PlayerInfo(int playerLevel, int playerCurrentEXP) { 
        this.playerLevel = playerLevel;
        this.playerCurrentEXP = playerCurrentEXP;
    }

    public int PlayerLevel { 
        get { return playerLevel; }
    }
    public int PlayerCurrentEXP { 
        get { return playerCurrentEXP; }    
    }

}
