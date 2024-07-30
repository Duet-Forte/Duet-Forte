
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;

public class EnemyAnimator : MonoBehaviour, IAnimator
{
    Animator theEnemyAnimator;
    
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
    string patternSignal = "PatternSignal";
    string readyToPatternSignal = "ReadyToPatternSignal";
    string die = "Die";
    string readyDash = "ReadyDash";
    #endregion
    #region ���� ��� �ߺ����� ����
    int randomAttackCase;
    int minDedupleAnim=0;
    int maxDedupleAnim;

    #endregion
    #region VFX ��ƼŬ ����
    [SerializeField] ParticleSystem[] AttackParticle;
    #endregion

    
    void Start()
    {
    
        theEnemyAnimator = GetComponent<Animator>();
        
        maxDedupleAnim = AttackParticle.Length;
    }
    #region �ִϸ��̼�
    public void ReadyToPatternSignal() {
        theEnemyAnimator.SetTrigger(readyToPatternSignal);
        
    }
    public void PatternSignal() {
        theEnemyAnimator.SetTrigger(patternSignal);
    }

    public void Dash()
    {
        theEnemyAnimator.SetBool(readyDash, true);
        theEnemyAnimator.SetTrigger(dash);
        Debug.Log("Enemy Dash");
    }
    public void BackDash()
    {
        theEnemyAnimator.SetBool(readyDash, false);
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
    private void DeduplicateAttack()//���� �ִϸ��̼ǰ� ��ƼŬ ���
    {
            randomAttackCase = Random.RandomRange(minDedupleAnim, maxDedupleAnim);
            theEnemyAnimator.SetFloat(attackCase, randomAttackCase);
            return;
    }
    #endregion
    #region ��ƼŬ
    #region �ִϸ��̼ǿ� ������ ��ƼŬ
    void EnemyAttackParticle(int attackCase)
    {
        if (AttackParticle[attackCase] != null)
        {
            ParticleSystem tmp = Instantiate<ParticleSystem>(AttackParticle[attackCase]);
            tmp.transform.SetParent(gameObject.transform);
            tmp.transform.localPosition = Vector3.zero;

        }
    }
    #endregion
    #endregion

    
    public void PlayerBeat()
    {

        theEnemyAnimator.SetTrigger(beat);

    }

 
}