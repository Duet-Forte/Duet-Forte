using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class PrepareTurnUI : InGameUI
{
    SkillSet theSkillSet;

    string[] skillSetName;//��ų�¿��� �޾ƿ� ��ų �̸�
    Sprite[] skillSetIcon;//��ų�¿��� �޾ƿ� ��ų ������
    List<string[]> skillSetCommand;//��ų�¿��� �޾ƿ� ��ų Ŀ�ǵ�
    string[] arrayOfParsedCommand = new string[4];//�޾ƿ� ��ų Ŀ�ǵ带 UI�� ǥ���� ��Ʈ������ �۾��� �� ���� ����
    [SerializeField] RectTransform[] arrayOfUITransform;//
    StageManager stageManager;

    #region UI�̵� ���� ����
    private float UIMoveTime = 0.3f;
    private float UIXOffset = 900f;

    #endregion
    //��ų ������, ��ų �̸�, ��ų Ŀ�ǵ� �� �ʱ�ȭ


  

    public void InitSetting()
    {
        arrayOfUITransform = new RectTransform[4];
        
        theSkillSet = FindObjectOfType<SkillSet>();
        skillSetIcon = theSkillSet.ArrayOfSkillIcon;
        skillSetName = theSkillSet.ArrayOfSkillName;
        skillSetCommand = theSkillSet.SkillCommand;

        SkillCommandParse();

        for (int Prepare_Skill_UI_Index = 0; Prepare_Skill_UI_Index < transform.childCount; Prepare_Skill_UI_Index++) //�� UI�� ��ų ������, �̸�, Ŀ�ǵ� �Է��ϴ� �ݺ���
        {
            arrayOfUITransform[Prepare_Skill_UI_Index] = transform.GetChild(Prepare_Skill_UI_Index).GetComponent<RectTransform>();
            transform.GetChild(Prepare_Skill_UI_Index).GetChild(0).GetComponent<Image>().sprite = skillSetIcon[Prepare_Skill_UI_Index];//������
            transform.GetChild(Prepare_Skill_UI_Index).GetChild(1).GetComponent<TMP_Text>().text = skillSetName[Prepare_Skill_UI_Index];//��ų�̸�
            transform.GetChild(Prepare_Skill_UI_Index).GetChild(2).GetComponent<TMP_Text>().text = arrayOfParsedCommand[Prepare_Skill_UI_Index];//��ųĿ�ǵ�

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
    public IEnumerator PopUpSkillUI()
    {
        //UI�˾�
        Debug.Log("PrepareTurn UI POPUP");
        for (int UIindex = 0; UIindex < arrayOfUITransform.Length; UIindex++)
        {
            arrayOfUITransform[UIindex].DOMove(new Vector2(arrayOfUITransform[UIindex].position.x + UIXOffset, arrayOfUITransform[UIindex].position.y)
                , UIMoveTime);

            yield return new WaitForSeconds(0.2f);
        }
    }
    public IEnumerator PopDownSkillUI()
    {
        //UI�˴ٿ�(?)
        Debug.Log("PrepareTurn UI POPDOWN");
        for (int UIindex = 0; UIindex < arrayOfUITransform.Length; UIindex++)
        {
            arrayOfUITransform[UIindex].DOMove(new Vector2(arrayOfUITransform[UIindex].position.x - UIXOffset, arrayOfUITransform[UIindex].position.y)
                ,UIMoveTime).SetEase(Ease.InBack);

            yield return new WaitForSeconds(0.2f);
        }
    }
    public void UISwitch(bool onOff) {
        if (onOff) { StartCoroutine(PopUpSkillUI()); }
        else if (!onOff) {
            StartCoroutine(PopDownSkillUI());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) { PopUpSkillUI(); }
        if (Input.GetKeyDown(KeyCode.K)) { PopDownSkillUI(); }
    }
}
