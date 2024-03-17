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
    string attackCase = "AttackCase";
    #endregion
    #region ���� ��� �ߺ����� ����
    int randomAttackCase;
    int minDedupleAnim=0;
    int maxDedupleAnim;
    int dedupleAnimCase;

    #endregion
    #region VFX ��ƼŬ ����
    [SerializeField] ParticleSystem[] AttackParticle;
    [SerializeField] ParticleSystem parryParticle;
    [SerializeField] ParticleSystem attackCountDownSignal;
    [SerializeField] ParticleSystem attackSignal;
    [SerializeField] ParticleSystem dashParticle;
    #endregion

    
    void Start()
    {
    
        theEnemyAnimator = GetComponent<Animator>();
        hitParticle = new HitParticle();
        maxDedupleAnim = AttackParticle.Length;
        dedupleAnimCase = Random.RandomRange(minDedupleAnim, maxDedupleAnim);
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
        DeduplicateAttack();


    }
    private void DeduplicateAttack()//���� �ִϸ��̼ǰ� ��ƼŬ ���
    {
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDedupleAnim);
            while (randomAttackCase == dedupleAnimCase)
            { //�ߺ����� ���� ������
                randomAttackCase = Random.RandomRange(minDedupleAnim, maxDedupleAnim);
            }
            theEnemyAnimator.SetFloat(attackCase, randomAttackCase);
            PlayAttackParticle(randomAttackCase);
            dedupleAnimCase = randomAttackCase;
            return;
    }
    #endregion
    #region ��ƼŬ
    #region �ִϸ��̼ǿ� ������ ��ƼŬ
    void PlayAttackParticle(int attackCase)
    {//�ִϸ��̼� Ŭ���� �̺�Ʈ�� �߰���. ���� �ִϸ��̼��� �߰��κп� �����Ŵ
        if (AttackParticle[attackCase] != null)
        {
            AttackParticle[attackCase].Play();
        }
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
