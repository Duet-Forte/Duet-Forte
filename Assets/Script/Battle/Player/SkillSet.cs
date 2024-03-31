using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어 오브젝트의 컴포넌트
public class SkillSet : MonoBehaviour
{
    
    [SerializeField] private PlayerSkill[] skillSet = new PlayerSkill[4]; //비전투씬에서 준비된 PlayerSkill(Scriptable Object로 만들어짐) 4개를 받아오면 작동됨\
    [SerializeField] private ParticleSystem[] arrayOfSkillParticle = new ParticleSystem[4];
    [SerializeField] string[][] arrayOfCommand;
    [SerializeField]private string[] arrayOfSkillName= new string[4];

    private AnimationClip[] arrayOfAnimationClip = new AnimationClip[4];
    [SerializeField]private List<string[]> skillCommand = new List<string[]>();
    private Sprite[] arrayOfSkillIcon=new Sprite[4];

    #region 프로퍼티
    public List<string[]> SkillCommand { get=>skillCommand; private set { skillCommand = value; } }//스트링 배열의 스킬 커맨드
    public string[] ArrayOfSkillName { get => arrayOfSkillName; }
    public PlayerSkill[] getSkillSet { get => skillSet; }
    
    public Sprite[] ArrayOfSkillIcon { get => arrayOfSkillIcon; }
    public ParticleSystem[] ArrayOfSkillParticle { get => arrayOfSkillParticle; }
    public AnimationClip[] ArrayOfAnimationClip { get => arrayOfAnimationClip; }
    #endregion

    void Awake()
    {
        //skillSet =~~//비전투 씬에서 세팅된 스킬을 받아올 예정
        for (int index = 0; index < skillSet.Length; index++) {//커맨드 리스트 할당
            skillCommand.Add(skillSet[index].SkillCommand);
            arrayOfCommand=skillCommand.ToArray();
            arrayOfSkillName[index] = skillSet[index].SkillName;
            arrayOfSkillIcon[index] = skillSet[index].SkillIcon;
            if (skillSet[index].SkillParticle != null)
            {
                ParticleSystem tmp = GameObject.Instantiate(skillSet[index].SkillParticle, transform.position, Quaternion.identity);
                tmp.transform.parent = gameObject.transform;
                tmp.gameObject.transform.localScale = gameObject.transform.localScale;
                //tmp.transform.SetParent(gameObject.transform, false);
                arrayOfSkillParticle[index] = tmp;
            }
            arrayOfAnimationClip[index] = skillSet[index].SkillAnimation;
            
        }
    }
    void PlaySkill() { 
    
    }
  
}
