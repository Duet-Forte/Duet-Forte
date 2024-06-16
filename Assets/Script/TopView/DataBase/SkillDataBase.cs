using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataBase
{
    private PlayerSkill[] data;
    private List<PlayerSkill> skill;
    private bool[] isActivated;
    public PlayerSkill[] Data
    {
        get
        {
            if (data == null)
                data = Resources.LoadAll<PlayerSkill>("Scriptable/Skill");
            return data;
        }
    }
    public PlayerSkill[] Skill
    {
        get
        {
            if (skill == null)
                skill = new List<PlayerSkill>();
            skill.Clear();

            for (int i = 0; i < Data.Length; ++i)
            {
                if (IsActivated[i])
                    skill.Add(data[i]);
            }

            return skill.ToArray();
        }
    }


    public bool[] IsActivated
    {
        get
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
    public void ActivateSkill(int ID)
    {
        Debug.Log(ID + "½ºÅ³ È¹µæ");
        isActivated[ID] = true;
    }
}
