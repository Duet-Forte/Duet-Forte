using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataBase
{
    private PlayerSkill[] data;
    private bool[] isActivated;
    public PlayerSkill[] Data { get 
    {
            if (data == null)
                data = Resources.LoadAll<PlayerSkill>("Scriptable/Skill");
            return data;
    } 
    }

    public bool[] IsActivated { get 
        {
            if (isActivated == null)
            {
                isActivated = new bool[data.Length];
                isActivated[0] = true;
                isActivated[1] = true;
            }
            return isActivated;
        } 
    }
    public void ActivateSkill(int ID) => isActivated[ID] = true;

}
