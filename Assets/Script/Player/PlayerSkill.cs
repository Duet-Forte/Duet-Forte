using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "Scriptable Object/PlayerSkill", order = 2)]
public class PlayerSkill : ScriptableObject
{
    //여기 있는 SerializeField 전부 지워야 됨
    //이름, 설명, 대미지, 커맨드, 
    [SerializeField] private string skillName;
    [SerializeField] private string skill_Information;
    [SerializeField] private int damage;
    [SerializeField] private string[] skillCommand;//"A" "B" "R"로 이루어짐 (일단은 커맨드는 엇박없이 정박으로만 이루어짐)
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private bool isSpacialSkill;
    [SerializeField] ParticleSystem skillParticle;
    [SerializeField] private AnimationClip skillAnimationClip;
    [SerializeField] private int coolTimeTurn;
    public string SkillName { get => skillName; }
    public int Damage { get => damage; }
    public string[] SkillCommand { get => skillCommand; } // 성재 : string을 사용해서 A,B,R 이렇게 사용하는 게 메모리 측면에서 이득을 볼 수 있을 듯 /재욱: 확인확인
    public ParticleSystem SkillParticle { get => skillParticle;  }
    public Sprite SkillIcon { get => skillIcon; }
   
    public AnimationClip SkillAnimation { get => skillAnimationClip; }

    public int CoolTimeTurn { get => coolTimeTurn; }
}
