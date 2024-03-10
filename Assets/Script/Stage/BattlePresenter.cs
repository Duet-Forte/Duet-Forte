using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;
using Util;

/// <summary>
/// Enemy�� Player���̿��� ����� ��ȯ VFX ���� ��� �����ϸ鼭 �Ͼ�� ���� ó������ �̾��ִ� ������ ������ �Ѵ�. 
/// </summary>
public class BattlePresenter
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
        
        
        this.stageManager = stageManager;
        enemy = this.stageManager.Enemy;
        enemyName = enemy.EnemyName;
        Debug.Log(enemyName);
        playerInterface = this.stageManager.PlayerInterface;
    }
    public void PlayerToEnemy(float playerAttack,bool isSlash) {
        int damage = 0;
        GetDefense();
        if (isSlash)
        {
            damage = DamageCalculate(playerAttack,enemySlashDefense);
            enemy.GetDamage(damage,isSlash);
            return;
        }
        if (!isSlash)
        {
            damage = DamageCalculate(playerAttack, enemyPierceDefense);
            enemy.GetDamage(damage,isSlash);
            return;
        }
        enemy.GetDamage(damage,isSlash);
    
    }

    public void EnemyToPlayer(float enemyAttack) {
        
        string enemyHitVFXPath = Const.ENEMY_VFX_PATH + enemyName+"/"+enemyName+"_BasicHit_VFX"; //enemy�� ����ϴ� vfx  
        Object.Instantiate<GameObject>(Resources.Load<GameObject>(enemyHitVFXPath),playerInterface.transform.position,Quaternion.identity);
        int damage;
        GetDefense();
        damage = DamageCalculate(enemyAttack,playerDefense);

        playerInterface.GetDamage(damage);
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
