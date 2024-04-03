using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "Scriptable Object/PlayerSkill", order = 2)]
public class PlayerSkill : ScriptableObject
{
    public struct Skill {
        public int[] damage;
        public bool[] damageType;
        public AnimationClip skillClip;
        public float[] waitTimes;
        public ParticleSystem skillParticle;
        
    }
    
    //이름, 설명, 대미지, 커맨드, 
    [SerializeField] private string skillName;
    [SerializeField] private string skill_Information;
    [SerializeField] private string[] skillCommand;//"A" "B" "R"로 이루어짐 (일단은 커맨드는 엇박없이 정박으로만 이루어짐)
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private bool isSpacialSkill;
    [SerializeField] private int coolTimeTurn;
    [Header("PlayerAttack class에서 처리할 데이터들")]
    [Space(1f)]
    [SerializeField] ParticleSystem skillParticle;
    [Tooltip("배열의 길이는 타수를 의미함 각 타수마다 대미지를 넣을 것")]
    [SerializeField] private int[] damage; //공격 타수가 배열 길이
    [Tooltip("각 float은 대미지가 들어가기까지 걸리는 시간을 의미함. 마지막 공격 이후 다음 공격까지 걸리는 시간을 입력")]
    [SerializeField] private float[] waitTimes;
    [Tooltip("각 공격의 대미지 타입을 의미함. (true는 베기공격, false는 찌르기 공격을 의미함.)")]
    [SerializeField] private bool[] damageType;
    [Tooltip("스킬에 사용되는 애니메이션 클립.")]
    [SerializeField] AnimationClip skillClip;
    public string SkillName { get => skillName; }
    public int[] Damage { get => damage; }
    public string[] SkillCommand { get => skillCommand; } // 성재 : string을 사용해서 A,B,R 이렇게 사용하는 게 메모리 측면에서 이득을 볼 수 있을 듯 /재욱: 확인확인
    public ParticleSystem SkillParticle { get => skillParticle;  }
    public Sprite SkillIcon { get => skillIcon; }

    public int CoolTimeTurn { get => coolTimeTurn; }
    
    public Skill GetSkill
    {
        get {
            Skill skill;
            skill.skillParticle = skillParticle;
            skill.damage = damage;
            skill.waitTimes = waitTimes;
            skill.skillClip = skillClip;
            skill.damageType = damageType;
            return skill;
            
        }
        
        } 
}

