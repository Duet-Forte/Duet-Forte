using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어 오브젝트의 컴포넌트
public class SkillSet : MonoBehaviour
{
    
    [SerializeField] private PlayerSkill[] skillSet = new PlayerSkill[4]; //비전투씬에서 준비된 PlayerSkill(Scriptable Object로 만들어짐) 4개를 받아오면 작동됨\
    [SerializeField] private ParticleSystem[] arrayOfSkillParticle = new ParticleSystem[4];
    [SerializeField]private string[] arrayOfSkillName= new string[4];
    private PlayerSkill.Skill[] arrayOfSkill=new PlayerSkill.Skill[4];
    [SerializeField]private List<string> skillCommand = new List<string>();
    private Sprite[] arrayOfSkillIcon=new Sprite[4];


    #region 프로퍼티
    public List<string> SkillCommand { get=>skillCommand; private set { skillCommand = value; } }//스트링 배열의 스킬 커맨드
    public string[] ArrayOfSkillName { get => arrayOfSkillName; }
    public PlayerSkill[] playerSkills { get => skillSet; }
    
    public Sprite[] ArrayOfSkillIcon { get => arrayOfSkillIcon; }
    public ParticleSystem[] ArrayOfSkillParticle { get => arrayOfSkillParticle; }
    public PlayerSkill.Skill[] ArrayOfSkill { get => arrayOfSkill; }
    #endregion

    /*void Awake()//테스트용으로 남겨둠.
    {
        //skillSet =~~//비전투 씬에서 세팅된 스킬을 받아올 예정
        for (int index = 0; index < skillSet.Length; index++) {//커맨드 리스트 할당
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
        {//커맨드 리스트 할당
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

