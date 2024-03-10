using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "Scriptable Object/PlayerSkill", order = 2)]
public class PlayerSkill : ScriptableObject
{
    //���� �ִ� SerializeField ���� ������ ��
    //�̸�, ����, �����, Ŀ�ǵ�, 
    [SerializeField] private string skillName;
    [SerializeField] private string skill_Information;
    [SerializeField] private int damage;
    [SerializeField] private string[] skillCommand;//"A" "B" "R"�� �̷���� (�ϴ��� Ŀ�ǵ�� ���ھ��� �������θ� �̷����)
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private bool isSpacialSkill;
    [SerializeField] ParticleSystem skillParticle;
    [SerializeField] private AnimationClip skillAnimationClip;
    [SerializeField] private int coolTimeTurn;
    public string SkillName { get => skillName; }
    public int Damage { get => damage; }
    public string[] SkillCommand { get => skillCommand; } // ���� : string�� ����ؼ� A,B,R �̷��� ����ϴ� �� �޸� ���鿡�� �̵��� �� �� ���� �� /���: Ȯ��Ȯ��
    public ParticleSystem SkillParticle { get => skillParticle;  }
    public Sprite SkillIcon { get => skillIcon; }
   
    public AnimationClip SkillAnimation { get => skillAnimationClip; }

    public int CoolTimeTurn { get => coolTimeTurn; }
}
