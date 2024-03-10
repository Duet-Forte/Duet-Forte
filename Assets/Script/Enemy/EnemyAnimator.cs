using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;

public class EnemyAnimator : MonoBehaviour, IAnimator
{
    Animator theEnemyAnimator;
    HitParticle hitParticle;
    // Start is called before the first frame update
    #region �ִϸ����� �Ķ���� �̸�
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

    #region VFX ��ƼŬ ����
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
    #region �ִϸ��̼�

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
    #region ��ƼŬ
    #region �ִϸ��̼ǿ� ������ ��ƼŬ
    void Particle_Slash_1()
    {//�ִϸ��̼� Ŭ���� �̺�Ʈ�� �߰���. ���� �ִϸ��̼��� �߰��κп� �����Ŵ
        
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

    #region ���ݽ�ȣ
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
