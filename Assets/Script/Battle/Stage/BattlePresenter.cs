using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;
using Util;

/// <summary>
/// Enemy�� Player���̿��� ����� ��ȯ VFX ���� ��� �����ϸ鼭 �Ͼ�� ���� ó������ �̾��ִ� ������ ������ �Ѵ�. 
/// </summary>
public class BattlePresenter : MonoBehaviour
{
    #region ����� �ʿ��� �ܺ� Ŭ���� �� ������
    StageManager stageManager;
    PlayerInterface playerInterface;
    IEnemy enemy;
    string enemyName;
    #endregion

    #region �÷��̾�� �� ����
    float playerDefense;
    float enemySlashDefense;
    float enemyPierceDefense;
    #endregion

    #region ��ƼŬ
    HitParticle hitParticle;
    #endregion
    
   
    public void InitSettings(StageManager stageManager) {

        hitParticle = new HitParticle();
        this.stageManager = stageManager;
        enemy = this.stageManager.Enemy;
        enemyName = enemy.EnemyName;
        Debug.Log(enemyName);
        playerInterface = this.stageManager.PlayerInterface;
    }
    public void PlayerBasicAttackToEnemy(float playerAttack,bool isSlash) {
        int damage = 0;
        GetDefense();
        if (isSlash)
        {
            damage = DamageCalculate(playerAttack,enemySlashDefense);
            enemy.GetDamage(damage);
            hitParticle.Generate_Player_Hit_Slash(enemy.Transform);
            return;
        }
        if (!isSlash)
        {
            damage = DamageCalculate(playerAttack, enemyPierceDefense);
            enemy.GetDamage(damage);
            hitParticle.Generate_Player_Hit_Pierce(enemy.Transform);
            return;
        }
    }
    public void PlayerSkillToEnemy(float playerAttack,bool isSlash) { 

        Debug.Log(playerAttack);
        enemy.GetDamage((int)playerAttack);
    
    }

    public void EnemyToPlayer(float enemyAttack) {
        
        string enemyHitVFXPath = Const.ENEMY_VFX_PATH + enemyName+"/"+enemyName+"_BasicHit_VFX"; //enemy�� ����ϴ� vfx  
        Object.Instantiate<GameObject>(Resources.Load<GameObject>(enemyHitVFXPath),playerInterface.transform.position,Quaternion.identity);
        int damage;
        GetDefense();
        damage = DamageCalculate(enemyAttack,playerDefense);

        playerInterface.GetDamage(damage);
    }

    public void GuardCounterToEnemy(float playerAttack) {
        StartCoroutine(GuardCounter(playerAttack));
        Object.Instantiate<GameObject>(Resources.Load<GameObject>("VFX/VFX_Prefab/Combat/Player/Hit/Player_CounterAttack_GuardCounter_Hit_VFX"),enemy.Transform.position, Quaternion.identity);

    }
    private void GuardCounterDamage() { 
    
    
    }
    IEnumerator GuardCounter(float playerAttack) {
        
        enemy.GetDamage((int)playerAttack);
        yield return new WaitForSeconds(0.45f);
        for (int guardCounterCount = 0; guardCounterCount < 10; guardCounterCount++) {
            enemy.GetDamage((int)playerAttack);
            yield return new WaitForSeconds(0.1f);

        }
    }
    /// <summary>
    /// ���ݷ°� ������ ������ ���ݷ�-������ �ݿø����Ѽ� int�� return�ϴ� �Լ�
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="defence"></param>
    /// <returns></returns>
    public int DamageCalculate(float attack, float defence = 0f)
    {
        if (attack < 0) { return 0; }
        if (defence < 0) { return (int)attack; }

        int calculatedDamage = 0;
        float tmpDamage = attack - defence;//�ݿø��� �ϱ� ���� �����ϴ� �ӽú���

        if (tmpDamage - (int)tmpDamage >= 0.5f)
        {//�Ǽ��ΰ� 0.5�̻��̸�
            calculatedDamage = (int)tmpDamage + 1;//�ø�
        }
        else
        {//0.5�̸��̸�
            calculatedDamage = (int)tmpDamage;//����
        }

        if (tmpDamage >= 0) //�Ϲ����� ����� ��� 
        {
            return calculatedDamage;
        }
        if (tmpDamage < 0)
        {
            return 0;
        }
        return 0;

    }

    void GetDefense()
    {

        Vector2 enemyDefenseVector = enemy.GetDefense();
        enemySlashDefense = enemyDefenseVector.x;
        enemyPierceDefense = enemyDefenseVector.y;
        playerDefense = playerInterface.PlayerStatus.PlayerDefence;


    }

}
