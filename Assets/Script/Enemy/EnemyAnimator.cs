using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;

public class EnemyAnimator : MonoBehaviour, IAnimator
{
    Animator theEnemyAnimator;
    HitParticle hitParticle;
    // Start is called before the first frame update
    #region 애니메이터 파라미터 이름
    string dash = "Dash";
    string backDash = "BackDash";
    string pierce = "Pierce";
    string guard = "Guard";
    string idle = "Idle";
    string hurt = "Hurt";
    string guardCounter = "GuardCounter";
    string beat = "Beat";
    string attack = "Attack";
    #endregion

    #region VFX 파티클 변수
    [SerializeField] ParticleSystem A_AttackParticle;
    [SerializeField] ParticleSystem B_AttackParticle;
    [SerializeField] ParticleSystem guardCounterParticle;
    [SerializeField] ParticleSystem parryParticle;
    [SerializeField] ParticleSystem attackCountDownSignal;
    [SerializeField] ParticleSystem attackSignal;
    [SerializeField] ParticleSystem dashParticle;
    #endregion

    
    void Start()
    {
    
        theEnemyAnimator = GetComponent<Animator>();
        hitParticle = new HitParticle(); 
    }
    #region 애니메이션

    public void Dash()
    {
        theEnemyAnimator.SetTrigger(dash);
        Debug.Log("Enemy Dash");
    }
    public void BackDash()
    {
        theEnemyAnimator.SetTrigger(backDash);
        Debug.Log("Enemy BackDash");
    }
    

    public void Guard()
    {
        theEnemyAnimator.SetTrigger(guard);
        Debug.Log("Enemy Guard");
    }
    public void Hurt(bool isSlash)
    {
        theEnemyAnimator.SetTrigger(hurt);
        if (isSlash)
        {
            hitParticle.Generate_Player_Hit_Slash(gameObject);
        }
        else {
            hitParticle.Generate_Player_Hit_Pierce(gameObject);
        }
        Debug.Log("Enemy Hurt");
    }
    

    
    public void Idle()
    {

        theEnemyAnimator.SetTrigger(idle);


    }
    public void Attack() {

        theEnemyAnimator.SetTrigger(attack);
        Particle_Slash_1();
    }
    #endregion
    #region 파티클
    #region 애니메이션에 참조된 파티클
    void Particle_Slash_1()
    {//애니메이션 클립에 이벤트로 추가함. 공격 애니메이션의 중간부분에 재생시킴
        
        ParticleSystem slashParticle = GameObject.Instantiate<ParticleSystem>(A_AttackParticle);
        slashParticle.transform.SetParent(gameObject.transform);
        slashParticle.transform.localPosition=Vector3.zero;
    }
    void Particle_Pierce_1()
    {
        B_AttackParticle.Play();
    }
    void Particle_GuardCount()
    {
        guardCounterParticle.Play();
    }
    
    public void Particle_DashDust()
    {
        dashParticle.Play();
        //Instantiate<ParticleSystem>(dashParticle).Play();
    }
    #endregion

    #region 공격신호
    public void attackCountDown()
    {
        attackCountDownSignal.Play();
    }
    public void attackSignalPlay()
    {
        attackSignal.Play();
    }
    #endregion

    #endregion

    public void PlayerBeat()
    {

        theEnemyAnimator.SetTrigger(beat);

    }

    public void skillAnimation(AnimationClip skillAnim)
    {
        //thePlayerAnimator



    }

    public void GenerateDashDust()
    {

    }
    //public void Generate
}
