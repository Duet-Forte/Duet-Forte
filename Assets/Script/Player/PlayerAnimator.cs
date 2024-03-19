using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;
public class PlayerAnimator : MonoBehaviour, IAnimator
{
    Animator thePlayerAnimator;

    // Start is called before the first frame update
    #region 애니메이터 파라미터 이름
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
    #endregion
    #region 애니메이션 중복방지
    int dedupleSlashAnimCase;//연속으로 같은 애니메이션 나오는 걸 방지하는 변수
    int deduplePierceAnimCase;
    int randomAttackCase;

    int minDedupleAnim;
    int maxDedupleSlashAnim;
    int maxDeduplePierceAnim;

    int endAttackAnim;
    #endregion
    #region VFX 파티클 변수
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

        minDedupleAnim = 1;
        maxDeduplePierceAnim = 3;
        maxDedupleSlashAnim = 5;
        endAttackAnim = -1;
        randomAttackCase = -1;

        deduplePierceAnimCase =Random.RandomRange(minDedupleAnim, maxDeduplePierceAnim);//애니메이션이 2종류이기 때문에
        dedupleSlashAnimCase = Random.RandomRange(minDedupleAnim, maxDedupleSlashAnim);//애니메이션이 4종류이기 때문에

    }
    #region 애니메이션

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
    

    /// <summary>
    /// 불 값에 따라 A공격과 B공격이 나뉨
    /// </summary>
    /// <param name="isSlash">true는 A버튼인 Slash, false는 B버튼인 Pierce 모션</param>
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
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDedupleSlashAnim);
            while (randomAttackCase == dedupleSlashAnimCase) { //중복되지 않을 때까지
            randomAttackCase= Random.RandomRange(minDedupleAnim, maxDedupleSlashAnim);
            }
            thePlayerAnimator.SetFloat(slashCase, randomAttackCase);
            dedupleSlashAnimCase = randomAttackCase;
            return;
        }
        if (!isSlash) {
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDeduplePierceAnim);
            while (randomAttackCase == deduplePierceAnimCase) { //중복되지 않을 때까지
                randomAttackCase = Random.RandomRange(minDedupleAnim, maxDeduplePierceAnim);
            }
            thePlayerAnimator.SetFloat(pierceCase, randomAttackCase);
            deduplePierceAnimCase = randomAttackCase;
            
            return;
        }
        
    
    }
    public void Idle() { 

        thePlayerAnimator.SetTrigger(idle);
      

    }
    #endregion
    #region 파티클
    #region 애니메이션에 참조된 파티클
    void Particle_Slash_1() { //애니메이션 클립에 이벤트로 추가함. 공격 애니메이션의 중간부분에 재생시킴
        A_AttackParticle01.Play();
    }
    void Particle_Slash_2()
    { //애니메이션 클립에 이벤트로 추가함. 공격 애니메이션의 중간부분에 재생시킴
        A_AttackParticle02.Play();
    }
    void Particle_Slash_3()
    { //애니메이션 클립에 이벤트로 추가함. 공격 애니메이션의 중간부분에 재생시킴
        A_AttackParticle03.Play();
    }
    void Particle_Slash_4()
    { //애니메이션 클립에 이벤트로 추가함. 공격 애니메이션의 중간부분에 재생시킴
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
    void PerfectParryingParticle() {
        HitParticle.GenerateParryHit(gameObject, true);
    }
    public void Particle_DashDust() {
        dashParticle.Play();
        //Instantiate<ParticleSystem>(dashParticle).Play();
    }
    #endregion

    #region 공격신호
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

    public void skillAnimation(AnimationClip skillAnim) {
        //thePlayerAnimator


        
    }

    public void GenerateDashDust() { 
    
    }
    //public void Generate
}
