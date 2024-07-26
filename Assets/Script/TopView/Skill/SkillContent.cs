using Util;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public class SkillContent : ScrollContent
{
    private Transform[] skillWindow;
    private Image[] skillIcons;
    private CurrentSkill[] currentSkills;
    private TextMeshProUGUI[] skillName;
    private SkillSaver skillSaver;
    private SkillSelector skillSelector;
    public void InitSettings(SkillSetter skillScroll, SkillSaver skillSaver)
    {
        pool = skillScroll.CurrentPool;
        skillSelector = skillScroll.SkillSelector;
        InitSetting(pool);

        skillWindow = new Transform[Const.CONTENT_IN_ROW];
        skillIcons = new Image[Const.CONTENT_IN_ROW];
        currentSkills = new CurrentSkill[Const.CONTENT_IN_ROW];
        skillName = new TextMeshProUGUI[Const.CONTENT_IN_ROW];

        this.skillSaver = skillSaver;

        for (int i = 0; i < Const.CONTENT_IN_ROW; ++i)
        {
            int index = i;
            skillWindow[i] = transform.GetChild(i);
            Transform skillApplyButton = skillWindow[i].Find("Icon");

            skillIcons[i] = skillApplyButton.GetComponent<Image>();
            currentSkills[i] = skillApplyButton.GetOrAddComponent<CurrentSkill>();

            skillName[i] = skillWindow[i].Find("Text").GetComponent<TextMeshProUGUI>();
        }
    }
    public override void RefreshContent(int currentRowIndex)
    {
        int currentIndex = currentRowIndex * Const.CONTENT_IN_ROW;
        int count = 0;
        if (currentRowIndex < ((DataBase.Skill.Data.Length - 1) / 3))
            count = Const.CONTENT_IN_ROW;
        else
            count = DataBase.Skill.Data.Length % Const.CONTENT_IN_ROW;

        for(int i = 0; i < count; ++i)
            skillWindow[i].gameObject.SetActive(true);
        for (int i = count; i < Const.CONTENT_IN_ROW; ++i)
        {
            if (i >= Const.CONTENT_IN_ROW)
                break;
            skillWindow[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < count; ++i)
        {
            int index = currentIndex + i;
            skillIcons[i].sprite = DataBase.Skill.Data[currentIndex + i].SkillIcon;
            skillName[i].text = DataBase.Skill.Data[currentIndex + i].SkillName;
            currentSkills[i].InitSettings(skillSelector, index);
            currentSkills[i].BindSaverEvent(skillSaver);
        }
    }
}
