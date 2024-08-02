using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataBase
{
    private PlayerSkill[] data;
    private SkillGainWindow skillGainWindow;
    private List<PlayerSkill> skill;
    private bool[] isActivated;
    public PlayerSkill[] Data
    {
        get
        {
            if (data == null)
            {
                PlayerSkill[] temp = Resources.LoadAll<PlayerSkill>("Scriptable/Skill");
                data = new PlayerSkill[temp.Length];
                for(int i =0; i < data.Length; ++i)
                {
                    data[temp[i].id] = temp[i];
                }
            }
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
                isActivated = new bool[Data.Length];
                isActivated[0] = true;
                isActivated[1] = true;
            }
            return isActivated;
        }
    }
    public void ActivateSkill(int ID)
    {
        if (IsActivated[ID] == true)
            return;
        if (skillGainWindow == null)
        {
            skillGainWindow = Object.Instantiate(Resources.Load<GameObject>("TopView/SkillGainWindow")).GetComponent<SkillGainWindow>();
        }
        skillGainWindow.InitSettings(data[ID]).Forget();
        isActivated[ID] = true;
    }
}