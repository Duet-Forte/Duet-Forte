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
    
    //�̸�, ����, �����, Ŀ�ǵ�, 
    [SerializeField] private string skillName;
    [SerializeField] private string skill_Information;
    [SerializeField] private string[] skillCommand;//"A" "B" "R"�� �̷���� (�ϴ��� Ŀ�ǵ�� ���ھ��� �������θ� �̷����)
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private bool isSpacialSkill;
    [SerializeField] private int coolTimeTurn;
    [Header("PlayerAttack class���� ó���� �����͵�")]
    [Space(1f)]
    [SerializeField] ParticleSystem skillParticle;
    [Tooltip("�迭�� ���̴� Ÿ���� �ǹ��� �� Ÿ������ ������� ���� ��")]
    [SerializeField] private int[] damage; //���� Ÿ���� �迭 ����
    [Tooltip("�� float�� ������� ������� �ɸ��� �ð��� �ǹ���. ������ ���� ���� ���� ���ݱ��� �ɸ��� �ð��� �Է�")]
    [SerializeField] private float[] waitTimes;
    [Tooltip("�� ������ ����� Ÿ���� �ǹ���. (true�� �������, false�� ��� ������ �ǹ���.)")]
    [SerializeField] private bool[] damageType;
    [Tooltip("��ų�� ���Ǵ� �ִϸ��̼� Ŭ��.")]
    [SerializeField] AnimationClip skillClip;
    public string SkillName { get => skillName; }
    public int[] Damage { get => damage; }
    public string[] SkillCommand { get => skillCommand; } // ���� : string�� ����ؼ� A,B,R �̷��� ����ϴ� �� �޸� ���鿡�� �̵��� �� �� ���� �� /���: Ȯ��Ȯ��
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

