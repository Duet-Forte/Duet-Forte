using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataBase
{
    private PlayerSkill[] data;
    public PlayerSkill[] Data { get 
        {
            if (data == null)
                data = Resources.LoadAll<PlayerSkill>("Scriptable/Skill");
            return data;
        } }


}
