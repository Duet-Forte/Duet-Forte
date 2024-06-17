using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class PrepareTurnUI : InGameUI
{
    SkillSet theSkillSet;

    //string[] skillSetName;//��ų�¿��� �޾ƿ� ��ų �̸�
    Sprite[] skillSetIcon;//��ų�¿��� �޾ƿ� ��ų ������
    List<string[]> skillSetCommand;//��ų�¿��� �޾ƿ� ��ų Ŀ�ǵ�
    string[] arrayOfParsedCommand = new string[4];//�޾ƿ� ��ų Ŀ�ǵ带 UI�� ǥ���� ��Ʈ������ �۾��� �� ���� ����
    [SerializeField] RectTransform[] arrayOfUITransform;//
    StageManager stageManager;

    #region UI�̵� ���� ����
    private float UIMoveTime = 0.3f;
    [SerializeField] private float defaultX;
    [SerializeField] private float popX;

    #endregion
    //��ų ������, ��ų �̸�, ��ų Ŀ�ǵ� �� �ʱ�ȭ




    public void InitSetting()
    {
        arrayOfUITransform = new RectTransform[4];
        
        theSkillSet = FindObjectOfType<SkillSet>();
        skillSetIcon = theSkillSet.ArrayOfSkillIcon;
        //skillSetName = theSkillSet.ArrayOfSkillName;
        skillSetCommand = theSkillSet.SkillCommand;

        SkillCommandParse();

        for (int Prepare_Skill_UI_Index = 0; Prepare_Skill_UI_Index < transform.childCount; Prepare_Skill_UI_Index++) //�� UI�� ��ų ������, �̸�, Ŀ�ǵ� �Է��ϴ� �ݺ���
        {
            
            if (theSkillSet.ArrayOfSkill != null)
            {
                Debug.Log(transform.GetChild(Prepare_Skill_UI_Index));
                transform.GetChild(Prepare_Skill_UI_Index).Find("Masking").Find("skillIcon").GetComponent<Image>().sprite = skillSetIcon[Prepare_Skill_UI_Index];//������
                //transform.GetChild(Prepare_Skill_UI_Index).GetChild(1).GetComponent<TMP_Text>().text = skillSetName[Prepare_Skill_UI_Index];//��ų�̸�
                transform.GetChild(Prepare_Skill_UI_Index).GetChild(1).GetComponent<TMP_Text>().text = arrayOfParsedCommand[Prepare_Skill_UI_Index];//��ųĿ�ǵ�
            }
            if (theSkillSet.getSkillSet.Length - 1 < Prepare_Skill_UI_Index)
            {
                Debug.Log("������� �ʴ� ��ųui ����");
                Destroy(transform.GetChild(Prepare_Skill_UI_Index).gameObject);
            }
            arrayOfUITransform[Prepare_Skill_UI_Index] = transform.GetChild(Prepare_Skill_UI_Index)?.GetComponent<RectTransform>();
        }
        

    }
    void SkillCommandParse()
    {//��ų�¿��� �޾ƿ� Ŀ�ǵ带 UI�� �Է��ϱ� ���� string���� ��ȯ�ϴ� �Լ�
        string tmpCommand = "";
        for (int skillSetIndex = 0; skillSetIndex < skillSetCommand.Count; skillSetIndex++)
        {
            for (int commandIndex = 0; commandIndex < skillSetCommand[skillSetIndex].Length; commandIndex++)
            {
                if (skillSetCommand[skillSetIndex][commandIndex] == "A" || skillSetCommand[skillSetIndex][commandIndex] == "B")//���߿� Ű ���ε��� �ϼ��Ǹ� ����
                {
                    tmpCommand += skillSetCommand[skillSetIndex][commandIndex];
                }
                else
                {
                    tmpCommand += "-"; //"R"�� "-"�� �ٲ�
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
