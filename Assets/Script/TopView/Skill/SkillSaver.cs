using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSaver : MenuSelector
{
    private Image[] skillIcons;
    private Vector2[] windowRects;
    private SkillSetter skillScroll;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCommand;
    [SerializeField] private Button saveButton;
    [SerializeField] private SelectPopUp applyPopUp;
    [SerializeField] private RectTransform selectImage;

    private bool isApplyPopUpEnabled;
    private int[] skillsInSlot;
    public void InitSettings(SkillSetter skillScroll)
    {
        base.InitSetting();
        this.skillScroll = skillScroll;
        skillsInSlot = new int[Menu.Length];
        skillIcons = new Image[Menu.Length];
        windowRects = new Vector2[Menu.Length];
        for (int i = 0; i < Menu.Length; ++i)
        {
            skillsInSlot[i] = -1;
            skillIcons[i] = Menu[i].GetComponent<Image>();
            windowRects[i] = Menu[i].GetComponent<RectTransform>().anchoredPosition;
        }

        isApplyPopUpEnabled = false;
        applyPopUp.InitSettings();
        applyPopUp.AddApplyClickEvent(() => isApplyPopUpEnabled = false);
        applyPopUp.AddCancelClickEvent(() => isApplyPopUpEnabled = false);

        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(() => ShowPopUp("현재 스킬을 저장하시겠습니까?"));
        selectImage.gameObject.SetActive(false);

        currentIndex = - 1;
        previousIndex = - 1;
    }

    public void RemoveCurrentSkill(int index)
    {
        if(skillsInSlot[index] == -1)
            return;
        skillsInSlot[index] = -1;
        selectImage.gameObject.SetActive(false);
        skillIcons[index].sprite = null;
    }

    public void SelectSkill(int playerSkillIndex)
    {
        ShowSkillDescription(playerSkillIndex);
        SetSkillSlot(playerSkillIndex);
    }

    public void SetSkillSlot(int playerSkillIndex)
    {
        if (playerSkillIndex == -1)
        {
            Debug.Log("선택한 스킬이 없습니다!");
            return;
        }

        int minWindowIndex = 3;
        for (int count = 0; count < skillsInSlot.Length; ++count)
        {
            if (playerSkillIndex == skillsInSlot[count])
            {
                Debug.Log("중복된 스킬이 존재합니다!");
                return;
            }
            if (skillsInSlot[count] == -1 && minWindowIndex > count)
            {
                minWindowIndex = count;
            }
        }
        skillIcons[minWindowIndex].sprite = DataBase.Skill.Data[playerSkillIndex].SkillIcon;
        skillIcons[minWindowIndex].color = new Color(skillIcons[minWindowIndex].color.r, skillIcons[minWindowIndex].color.g, skillIcons[minWindowIndex].color.b, 1);
        skillsInSlot[minWindowIndex] = playerSkillIndex;
    }

    public void ShowSkillDescription(int playerSkillIndex)
    {
        if (playerSkillIndex < 0)
        {
            skillDescription.text = string.Empty;
            skillCommand.text = string.Empty;
            return;
        }
        PlayerSkill selectedPlayerSkill = DataBase.Skill.Data[playerSkillIndex];
        skillDescription.text = selectedPlayerSkill.Information;
        skillCommand.text = selectedPlayerSkill.SkillCommand.ToString();
    }
    public void ShowSkillDescriptionInWindow(int index)
    {
        selectImage.gameObject.SetActive(true);
        selectImage.anchoredPosition = windowRects[index];
        int currentWindowIndex = skillsInSlot[index];
        ShowSkillDescription(currentWindowIndex);
    }

    public void HideSkillDescription(int index) => selectImage.gameObject.SetActive(false);
    private void ShowPopUp(string guideText)
    {
        applyPopUp.ChangeGuideText(guideText);
        applyPopUp.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (skillScroll.IsSelectorSelected || isApplyPopUpEnabled)
            return;
        if(currentIndex < 0)
        {
            currentIndex = 0;
            SetIndex(currentIndex);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int changedIndex = currentIndex - 1;
            SetIndex(changedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            int changedIndex = currentIndex + 1;
            SetIndex(changedIndex);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            menuArray[currentIndex].OnPressed();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowPopUp("저장하시겠습니까?");
            isApplyPopUpEnabled = true;
        }
    }

    public override void SetIndex(int index)
    {
        if (index >= menuArray.Length)
            return;
        if (index < 0)
        {
            skillScroll.IsSelectorSelected = true;
            menuArray[currentIndex].OnDeselected();
            previousIndex = -1;
            currentIndex = -1;
            return;
        }
        previousIndex = currentIndex;
        menuArray[previousIndex].OnDeselected();
        currentIndex = index;
        menuArray[currentIndex].OnSelected();
    }
}