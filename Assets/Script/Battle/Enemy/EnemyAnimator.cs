
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;

public class EnemyAnimator : MonoBehaviour, IAnimator
{
    Animator theEnemyAnimator;
    
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
    string attackCase = "AttackCase";
    string patternSignal = "PatternSignal";
    string readyToPatternSignal = "ReadyToPatternSignal";
    string die = "Die";
    #endregion
    #region 공격 모션 중복방지 변수
    int randomAttackCase;
    int minDedupleAnim=0;
    int maxDedupleAnim;

    #endregion
    #region VFX 파티클 변수
    [SerializeField] ParticleSystem[] AttackParticle;
    
    [SerializeField] ParticleSystem attackCountDownSignal;
    [SerializeField] ParticleSystem attackSignal;
    [SerializeField] ParticleSystem dashParticle;
    #endregion

    
    void Start()
    {
    
        theEnemyAnimator = GetComponent<Animator>();
        
        maxDedupleAnim = AttackParticle.Length;
    }
    #region 애니메이션
    public void ReadyToPatternSignal() {
        theEnemyAnimator.SetTrigger(readyToPatternSignal);
        
    }
    public void PatternSignal() {
        theEnemyAnimator.SetTrigger(patternSignal);
    }

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
    public void Die() {
        theEnemyAnimator.SetTrigger(die);
        Debug.Log("Enemy Die");
    }
    

    public void Guard()
    {
        theEnemyAnimator.SetTrigger(guard);
        Debug.Log("Enemy Guard");
    }
    public void Hurt()
    {
        theEnemyAnimator.SetTrigger(hurt);
        
        Debug.Log("Enemy Hurt");
    }
    

    
    public void Idle()
    {

        theEnemyAnimator.SetTrigger(idle);
        Debug.Log("Enemy Idle");

    }
    public void Attack() {

        theEnemyAnimator.SetTrigger(attack);
        DeduplicateAttack();


    }
    private void DeduplicateAttack()//공격 애니메이션과 파티클 재생
    {
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDedupleAnim);
            theEnemyAnimator.SetFloat(attackCase, randomAttackCase);
            return;
    }
    #endregion
    #region 파티클
    #region 애니메이션에 참조된 파티클
    public void Particle_DashDust()
    {
        dashParticle.Play();
        //Instantiate<ParticleSystem>(dashParticle).Play();
    }
    void PlayAttackParticle(int attackCase)
    {
        if (AttackParticle[attackCase] != null)
        {
            ParticleSystem tmp = Instantiate<ParticleSystem>(AttackParticle[attackCase]);
            tmp.transform.SetParent(gameObject.transform);
            tmp.transform.localPosition = Vector3.zero;

        }
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

 
}
