using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    int playerLevel;
    int playerCurrentEXP;
    PlayerSkill[] playerSkills;
    int playerHP;
    private int[] epuippedItem;
    #region 생성자 모음
    public PlayerInfo(int playerLevel, int playerCurrentEXP) 
    {
        this.playerLevel = playerLevel;
        this.playerCurrentEXP = playerCurrentEXP;
    }
    public PlayerInfo(PlayerSkill[] playerSkills, int playerLevel, int playerCurrentEXP,int[] equippedItem)
    {
        this.playerSkills = playerSkills;
        this.playerCurrentEXP = playerCurrentEXP;
        this.playerLevel = playerLevel;
        this.epuippedItem = equippedItem;

    }
    public PlayerInfo(PlayerSkill[] playerSkills, int playerLevel, int playerCurrentEXP)
    {
        this.playerSkills = playerSkills;
        this.playerCurrentEXP = playerCurrentEXP;
        this.playerLevel = playerLevel;
    }
    public PlayerInfo(int playerLevel, int playerCurrentEXP,int playerHP)
    {
        this.playerSkills=playerSkills;
        this.playerCurrentEXP = playerCurrentEXP;
        this.playerLevel = playerLevel;
        this.playerHP = playerHP;
    }

    public PlayerInfo(PlayerSkill[] playerSkills, int playerLevel, int playerCurrentExp, int playerHp)
    {
        this.playerSkills=playerSkills;
        this.playerCurrentEXP = playerCurrentEXP;
        this.playerLevel = playerLevel;
        this.playerHP = playerHP;
    }
    public PlayerInfo(PlayerSkill[] playerSkills, int playerLevel, int playerCurrentExp, int playerHp,int[] equippedItem)
    {
        
        this.playerSkills=playerSkills;
        this.playerCurrentEXP = playerCurrentEXP;
        this.playerLevel = playerLevel;
        this.playerHP = playerHP;
        this.epuippedItem = epuippedItem;
    }

    #endregion
    public int PlayerLevel 
    {
        get { return playerLevel; }
    }
    public int PlayerCurrentEXP 
    {
        get { return playerCurrentEXP; }
    }
    public PlayerSkill[] PlayerSkills
    {
        get { return playerSkills; }
    }
    public int PlayerHP 
    { 
    get { return playerHP; }
    }

    public int[] EquippedItem
    {
        get { return epuippedItem; }
    }

}
