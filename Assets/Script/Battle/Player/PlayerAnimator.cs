using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;
public class PlayerAnimator : MonoBehaviour, IAnimator
{
    Animator thePlayerAnimator;
    AnimatorOverrideController animatorOverrideController;
    // Start is called before the first frame update
    #region �ִϸ����� �Ķ���� �̸�
    string dash = "Dash";
    string slash = "Slash";
    string backDash = "BackDash";
    string pierce = "Pierce";
    string readyToParry = "ReadyToParry";
    string parry = "Parry";
    string idle = "Idle";
    string hurt = "Hurt";
    string guardCounter = "GuardCounter";
    string beat = "Beat";
    string slashCase = "SlashCase";
    string pierceCase = "PierceCase";
    string canAttack = "CanAttack";
    string skill = "Skill";
    #endregion
    #region �ִϸ��̼� �ߺ�����
    int randomAttackCase=0;

    int minDedupleAnim;
    int maxDedupleSlashAnim;
    int maxDeduplePierceAnim;

    int endAttackAnim;
    #endregion
    #region VFX ��ƼŬ ����
    [SerializeField] ParticleSystem A_AttackParticle01;
    [SerializeField] ParticleSystem A_AttackParticle02;
    [SerializeField] ParticleSystem A_AttackParticle03;
    [SerializeField] ParticleSystem A_AttackParticle04;
    [SerializeField] ParticleSystem B_AttackParticle01;
    [SerializeField] ParticleSystem B_AttackParticle02;
    [SerializeField] ParticleSystem guardCounterParticle;
    [SerializeField] ParticleSystem parryParticle;
    [SerializeField] ParticleSystem attackCountDownSignal;
    [SerializeField] ParticleSystem attackSignal;
    [SerializeField] ParticleSystem dashParticle;
    #endregion

    BattleDirector director;
    SpacialAttack spacialAttack;
    HitParticle  HitParticle;
    void Start()
    {
        director = new BattleDirector();
        spacialAttack = new SpacialAttack();
        HitParticle = new HitParticle();
        thePlayerAnimator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController();
        animatorOverrideController.runtimeAnimatorController = thePlayerAnimator.runtimeAnimatorController;
        minDedupleAnim = 1;
        maxDeduplePierceAnim = 3;
        maxDedupleSlashAnim = 5;
        endAttackAnim = -1;
        randomAttackCase = 0;

       

    }
    #region �ִϸ��̼�

    public void Dash() {
        thePlayerAnimator.SetTrigger(dash);
        Debug.Log("Player Dash");
    }
    public void BackDash()
    {
        thePlayerAnimator.SetTrigger(backDash);
        Debug.Log("Player BackDash");
    }
    public void Guard() {
        thePlayerAnimator.SetTrigger(readyToParry);
        Debug.Log("Player ReadyToParry");
    }

    public void Parry(bool isPerfect) {
        thePlayerAnimator.SetTrigger(parry);
        Particle_Parry(isPerfect);
        Debug.Log("Player Parry");
    }
    public void Hurt() {
        thePlayerAnimator.SetTrigger(hurt);
        Debug.Log("Player Hurt");
    }
    public void guardCount() {
        thePlayerAnimator.SetTrigger(guardCounter);
        spacialAttack.GenerateGuardCounterSlash(gameObject);
        director.Rush(gameObject, new Vector2(gameObject.transform.localPosition.x+35, gameObject.transform.localPosition.y), 0.025f, DG.Tweening.Ease.OutExpo);
        Debug.Log("Player GuardCounter");
    }

    public void Skill(AnimationClip skillClip) {
        thePlayerAnimator.SetTrigger(skill);
        animatorOverrideController["DummySkill"] = skillClip;
        thePlayerAnimator.runtimeAnimatorController = animatorOverrideController;
        
        
    }

    /// <summary>
    /// �� ���� ���� A���ݰ� B������ ����
    /// </summary>
    /// <param name="isSlash">true�� A��ư�� Slash, false�� B��ư�� Pierce ���</param>
    public void Attack(bool isSlash) {

        if (isSlash) {



            thePlayerAnimator.SetTrigger(slash);
            DeduplicateAttack(isSlash);

            return;
        }

        thePlayerAnimator.SetTrigger(pierce);
        DeduplicateAttack(isSlash);
        return;
    }

    private void DeduplicateAttack(bool isSlash) {
        
        if (isSlash) {
            thePlayerAnimator.SetFloat(slashCase, randomAttackCase);
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDedupleSlashAnim);
            return;
        }
        if (!isSlash) {
            thePlayerAnimator.SetFloat(pierceCase, randomAttackCase);
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDeduplePierceAnim);
            return;
        }


    }
    public void Idle() { 
        thePlayerAnimator.SetTrigger(idle);
    }
    #endregion
    #region ��ƼŬ
    #region �ִϸ��̼ǿ� ������ ��ƼŬ
    void Particle_Slash_1() { //�ִϸ��̼� Ŭ���� �̺�Ʈ�� �߰���. ���� �ִϸ��̼��� �߰��κп� �����Ŵ
        A_AttackParticle01.Play();
    }
    void Particle_Slash_2()
    { //�ִϸ��̼� Ŭ���� �̺�Ʈ�� �߰���. ���� �ִϸ��̼��� �߰��κп� �����Ŵ
        A_AttackParticle02.Play();
    }
    void Particle_Slash_3()
    { //�ִϸ��̼� Ŭ���� �̺�Ʈ�� �߰���. ���� �ִϸ��̼��� �߰��κп� �����Ŵ
        A_AttackParticle03.Play();
    }
    void Particle_Slash_4()
    { //�ִϸ��̼� Ŭ���� �̺�Ʈ�� �߰���. ���� �ִϸ��̼��� �߰��κп� �����Ŵ
        A_AttackParticle04.Play();
    }

    void Particle_Pierce_1() {
        B_AttackParticle01.Play();
    }
    void Particle_Pierce_2()
    {
        B_AttackParticle02.Play();
    }
    void Particle_GuardCount() {
        guardCounterParticle.Play();
    }
    void EndAttackState() {
        thePlayerAnimator.SetInteger(pierceCase, -1);
        thePlayerAnimator.SetInteger(slashCase, -1);
    }
    void Particle_Parry(bool isPerfect) {
        if (isPerfect)
        {
            HitParticle.GenerateParryHit(gameObject,isPerfect);
        }
        if (!isPerfect) {
            HitParticle.GenerateParryHit(gameObject,isPerfect);
        }
    }
   
    public void Particle_DashDust() {
        dashParticle.Play();
        //Instantiate<ParticleSystem>(dashParticle).Play();
    }
    #endregion

    #region ���ݽ�ȣ
    public void attackCountDown() {
        attackCountDownSignal.Play();
    }
    public void attackSignalPlay() {
        attackSignal.Play();
    }
    #endregion

    #endregion

    public void PlayerBeat() {

        thePlayerAnimator.SetTrigger(beat);

    }

   
}
