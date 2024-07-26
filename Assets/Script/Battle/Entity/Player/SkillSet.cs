using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�÷��̾� ������Ʈ�� ������Ʈ
public class SkillSet : MonoBehaviour
{
    
    [SerializeField] private PlayerSkill[] skillSet = new PlayerSkill[4]; //������������ �غ�� PlayerSkill(Scriptable Object�� �������) 4���� �޾ƿ��� �۵���\
    [SerializeField] private ParticleSystem[] arrayOfSkillParticle = new ParticleSystem[4];
    [SerializeField]private string[] arrayOfSkillName= new string[4];
    private PlayerSkill.Skill[] arrayOfSkill=new PlayerSkill.Skill[4];
    [SerializeField]private List<string> skillCommand = new List<string>();
    private Sprite[] arrayOfSkillIcon=new Sprite[4];


    #region ������Ƽ
    public List<string> SkillCommand { get=>skillCommand; private set { skillCommand = value; } }//��Ʈ�� �迭�� ��ų Ŀ�ǵ�
    public string[] ArrayOfSkillName { get => arrayOfSkillName; }
    public PlayerSkill[] playerSkills { get => skillSet; }
    
    public Sprite[] ArrayOfSkillIcon { get => arrayOfSkillIcon; }
    public ParticleSystem[] ArrayOfSkillParticle { get => arrayOfSkillParticle; }
    public PlayerSkill.Skill[] ArrayOfSkill { get => arrayOfSkill; }
    #endregion

    /*void Awake()//�׽�Ʈ������ ���ܵ�.
    {
        //skillSet =~~//������ ������ ���õ� ��ų�� �޾ƿ� ����
        for (int index = 0; index < skillSet.Length; index++) {//Ŀ�ǵ� ����Ʈ �Ҵ�
            if (skillSet[index] != null)
            {
                skillCommand.Add(skillSet[index].SkillCommand);
                arrayOfCommand = skillCommand.ToArray();
                arrayOfSkillName[index] = skillSet[index].SkillName;
                arrayOfSkillIcon[index] = skillSet[index].SkillIcon;
                arrayOfSkill[index] = skillSet[index].GetSkill;
            }
        }
        
    }*/
    
  public void InitSettings(PlayerSkill[] skillSet) {
        if (skillSet == null) return;
        this.skillSet = skillSet;
        for (int index = 0; index < skillSet.Length; index++)
        {//Ŀ�ǵ� ����Ʈ �Ҵ�
            if (skillSet[index] != null)
            {
                skillCommand.Add(skillSet[index].SkillCommand);
                arrayOfSkillName[index] = skillSet[index].SkillName;
                arrayOfSkillIcon[index] = skillSet[index].SkillIcon;
                arrayOfSkill[index] = skillSet[index].GetSkill;
            }

        }
    }
}

