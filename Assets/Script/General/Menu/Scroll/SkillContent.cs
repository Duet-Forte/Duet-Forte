using Util;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Pool;

public class SkillContent : ScrollContent
{
    private Transform[] skillWindow;
    private Image[] skillIcons;
    private Button[] buttons;
    private TextMeshProUGUI[] skillName;

    private PlayerSkill[] playerSkills;
    public void InitSettings(PlayerSkill[] skillData, ObjectPool<ScrollContent> pool)
    {
        InitSetting(pool);
        playerSkills = skillData;

        skillWindow = new Transform[Const.CONTENT_IN_ROW];
        skillIcons = new Image[Const.CONTENT_IN_ROW];
        buttons = new Button[Const.CONTENT_IN_ROW];
        skillName = new TextMeshProUGUI[Const.CONTENT_IN_ROW];

        for (int i = 0; i < Const.CONTENT_IN_ROW; ++i)
        {
            skillWindow[i] = transform.GetChild(i);
            Transform skillApplyButton = skillWindow[i].Find("Icon");

            skillIcons[i] = skillApplyButton.GetComponent<Image>();
            buttons[i] = skillApplyButton.GetComponent<Button>();
            skillName[i] = skillWindow[i].Find("Text").GetComponent<TextMeshProUGUI>();
        }
    }
    public override void RefreshContent(int currentRowIndex)
    {
        int currentIndex = currentRowIndex * Const.CONTENT_IN_ROW;
        if (currentRowIndex < ((playerSkills.Length - 1) / 3))
        {
            foreach (Transform transform in skillWindow)
                transform.gameObject.SetActive(true);
            for (int i = 0; i < Const.CONTENT_IN_ROW; ++i)
            {
                skillIcons[i].sprite = playerSkills[currentIndex + i].SkillIcon;
                skillName[i].text = playerSkills[currentIndex + i].SkillName;
                Debug.Log(skillName[i].text);
            }
            Debug.Log($"{currentRowIndex}줄 갱신됨!");
        }
        else
        {
            int count = playerSkills.Length % Const.CONTENT_IN_ROW;
            for (int i = Const.CONTENT_IN_ROW - 1; i >= count; --i)
            {
                skillWindow[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < count; ++i)
            {
                skillIcons[i].sprite = playerSkills[currentIndex + i].SkillIcon;
                skillName[i].text = playerSkills[currentIndex + i].SkillName;
            }
            Debug.Log($"마지막 줄 갱신됨!");
        }
    }
}
