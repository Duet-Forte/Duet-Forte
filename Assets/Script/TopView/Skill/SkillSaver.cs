using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSaver : MenuSelector
{
    private Image[] skillIcons;
    private Vector2[] windowRects;
    [SerializeField] private int skillWindowIndex;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCommand;
    [SerializeField] private Button saveButton;
    [SerializeField] private SelectPopUp applyPopUp;
    [SerializeField] private RectTransform selectImage;

    private int selectedPlayerSkillIndex;
    private int[] skillsInSlot;
    public override void InitSetting()
    {
        base.InitSetting();

        skillsInSlot = new int[Menu.Length];
        skillIcons = new Image[Menu.Length];
        windowRects = new Vector2[Menu.Length];
        skillWindowIndex = -1;
        for (int i = 0; i < Menu.Length; ++i)
        {
            skillsInSlot[i] = -1;
            skillIcons[i] = Menu[i].GetComponent<Image>();
            windowRects[i] = Menu[i].GetComponent<RectTransform>().anchoredPosition;
        }
        applyPopUp.InitSettings();
        applyPopUp.AddApplyClickEvent(SetSkillSlot);
        applyPopUp.AddApplyClickEvent(ResetSkillIndex);
        applyPopUp.AddCancelClickEvent(ResetSkillIndex);

        saveButton.onClick.RemoveAllListeners();
        selectedPlayerSkillIndex = -1;
        selectImage.gameObject.SetActive(false);
    }

    public void ChangeCurrentSkillWindowIndex(int index)
    {
        if (skillWindowIndex == index)
        {
            skillWindowIndex = -1;
            selectImage.gameObject.SetActive(false);
        }
        else
        {
            skillWindowIndex = index;
            selectImage.gameObject.SetActive(true);
            selectImage.anchoredPosition = windowRects[skillWindowIndex];
            if (selectedPlayerSkillIndex >= 0)
                ShowSkillApplyPopUp();
            int currentWindowIndex = skillsInSlot[index];
            ShowSkillDescription(currentWindowIndex);
        }
    }

    public void SelectSkill(int playerSkillIndex)
    {
        selectedPlayerSkillIndex = playerSkillIndex;
        if (skillWindowIndex >= 0)
            ShowSkillApplyPopUp();
        ShowSkillDescription(playerSkillIndex);
    }

    public void SetSkillSlot()
    {
        if (skillWindowIndex == -1)
            return;
        foreach (var skill in skillsInSlot)
        {
            if (selectedPlayerSkillIndex == -1)
            {
                Debug.Log("선택한 스킬이 없습니다!");
                return;
            }
            else if (selectedPlayerSkillIndex == skill)
            {
                Debug.Log("중복된 스킬이 존재합니다!");
                return;
            }
        }
        skillIcons[skillWindowIndex].sprite = DataBase.Instance.Skill.Data[selectedPlayerSkillIndex].SkillIcon;
        skillIcons[skillWindowIndex].color = new Color(skillIcons[skillWindowIndex].color.r, skillIcons[skillWindowIndex].color.g, skillIcons[skillWindowIndex].color.b, 1);
        skillsInSlot[skillWindowIndex] = selectedPlayerSkillIndex;
    }

    private void ShowSkillDescription(int playerSkillIndex)
    {
        if (playerSkillIndex < 0)
        {
            skillDescription.text = string.Empty;
            skillCommand.text = string.Empty;
            return;
        }
        PlayerSkill selectedPlayerSkill = DataBase.Instance.Skill.Data[playerSkillIndex];
        skillDescription.text = selectedPlayerSkill.Information;
        skillCommand.text = selectedPlayerSkill.SkillCommand.ToString();
        selectedPlayerSkill = null;
    }

    private void ShowSkillApplyPopUp() => ShowPopUp("해당 스킬을 선택하시겠습니까?");
    private void ShowPopUp(string guideText)
    {
        applyPopUp.ChangeGuideText(guideText);
        applyPopUp.gameObject.SetActive(true);
    }
        private void ResetSkillIndex()
    {
        selectedPlayerSkillIndex = -1;
        skillWindowIndex = -1;
        ShowSkillDescription(selectedPlayerSkillIndex);
        ChangeCurrentSkillWindowIndex(skillWindowIndex);
    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int changedIndex = currentIndex - 1;
            SetIndex(changedIndex);
            menuArray[currentIndex].OnPressed();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            int changedIndex = currentIndex + 1;
            SetIndex(changedIndex);
            menuArray[currentIndex].OnPressed();
        }
    }
}