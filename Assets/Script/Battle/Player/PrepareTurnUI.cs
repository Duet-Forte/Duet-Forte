using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class PrepareTurnUI : InGameUI
{
    SkillSet theSkillSet;

    //string[] skillSetName;//스킬셋에서 받아온 스킬 이름
    Sprite[] skillSetIcon;//스킬셋에서 받아온 스킬 아이콘
    List<string[]> skillSetCommand;//스킬셋에서 받아온 스킬 커맨드
    string[] arrayOfParsedCommand = new string[4];//받아온 스킬 커맨드를 UI에 표기할 스트링으로 작업한 후 담을 변수
    [SerializeField] RectTransform[] arrayOfUITransform;//
    StageManager stageManager;

    #region UI이동 관련 변수
    private float UIMoveTime = 0.3f;
    [SerializeField] private float defaultX;
    [SerializeField] private float popX;

    #endregion
    //스킬 아이콘, 스킬 이름, 스킬 커맨드 등 초기화




    public void InitSetting()
    {
        arrayOfUITransform = new RectTransform[4];
        
        theSkillSet = FindObjectOfType<SkillSet>();
        skillSetIcon = theSkillSet.ArrayOfSkillIcon;
        //skillSetName = theSkillSet.ArrayOfSkillName;
        skillSetCommand = theSkillSet.SkillCommand;

        SkillCommandParse();

        for (int Prepare_Skill_UI_Index = 0; Prepare_Skill_UI_Index < transform.childCount; Prepare_Skill_UI_Index++) //각 UI에 스킬 아이콘, 이름, 커맨드 입력하는 반복문
        {
            
            if (theSkillSet.ArrayOfSkill != null)
            {
                Debug.Log(transform.GetChild(Prepare_Skill_UI_Index));
                transform.GetChild(Prepare_Skill_UI_Index).Find("Masking").Find("skillIcon").GetComponent<Image>().sprite = skillSetIcon[Prepare_Skill_UI_Index];//아이콘
                //transform.GetChild(Prepare_Skill_UI_Index).GetChild(1).GetComponent<TMP_Text>().text = skillSetName[Prepare_Skill_UI_Index];//스킬이름
                transform.GetChild(Prepare_Skill_UI_Index).GetChild(1).GetComponent<TMP_Text>().text = arrayOfParsedCommand[Prepare_Skill_UI_Index];//스킬커맨드
            }
            if (theSkillSet.getSkillSet.Length - 1 < Prepare_Skill_UI_Index)
            {
                Debug.Log("사용하지 않는 스킬ui 제거");
                Destroy(transform.GetChild(Prepare_Skill_UI_Index).gameObject);
            }
            arrayOfUITransform[Prepare_Skill_UI_Index] = transform.GetChild(Prepare_Skill_UI_Index)?.GetComponent<RectTransform>();
        }
        

    }
    void SkillCommandParse()
    {//스킬셋에서 받아온 커맨드를 UI에 입력하기 위한 string으로 변환하는 함수
        string tmpCommand = "";
        for (int skillSetIndex = 0; skillSetIndex < skillSetCommand.Count; skillSetIndex++)
        {
            for (int commandIndex = 0; commandIndex < skillSetCommand[skillSetIndex].Length; commandIndex++)
            {
                if (skillSetCommand[skillSetIndex][commandIndex] == "A" || skillSetCommand[skillSetIndex][commandIndex] == "B")//나중에 키 바인딩이 완성되면 수정
                {
                    tmpCommand += skillSetCommand[skillSetIndex][commandIndex];
                }
                else
                {
                    tmpCommand += "-"; //"R"을 "-"로 바꿈
                }
                tmpCommand += "  ";
            }
            arrayOfParsedCommand[skillSetIndex] = tmpCommand;
            tmpCommand = "";


        }


    }
    public void applyCoolTimeTurn() { 
        


    }
    public IEnumerator AppearSkillUI()
    {
        Debug.Log("PrepareTurn UI POPUP");
        for (int UIindex = 0; UIindex < arrayOfUITransform.Length; UIindex++)
        {
            if (arrayOfUITransform[UIindex] != null)
            arrayOfUITransform[UIindex].DOAnchorPosX(popX, UIMoveTime);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public IEnumerator DisappearSkillUI()
    {   
        Debug.Log("PrepareTurn UI POPDOWN");
        for (int UIindex = 0; UIindex < arrayOfUITransform.Length; UIindex++)
        {
            if (arrayOfUITransform[UIindex] != null)
                arrayOfUITransform[UIindex]?.DOAnchorPosX(defaultX, UIMoveTime).SetEase(Ease.InBack);

            yield return new WaitForSeconds(0.2f);
        }
    }
    public void UISwitch(bool onOff) {
        if (onOff) { StartCoroutine(AppearSkillUI()); }
        else if (!onOff) {
            StartCoroutine(DisappearSkillUI());
        }
    }

    
}
