using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;
using Util;

/// <summary>
/// Enemy와 Player사이에서 대미지 교환 VFX 전달 등등 전투하면서 일어나는 각종 처리들을 이어주는 진행자 역할을 한다. 
/// </summary>
public class BattlePresenter
{
    #region 중재시 필요한 외부 클래스 및 변수들
    StageManager stageManager;
    PlayerInterface playerInterface;
    IEnemy enemy;
    string enemyName;
    #endregion

    #region 플레이어와 적 스탯
    float playerDefense;
    float enemySlashDefense;
    float enemyPierceDefense;
    #endregion

    #region 파티클
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
        
        string enemyHitVFXPath = Const.ENEMY_VFX_PATH + enemyName+"/"+enemyName+"_BasicHit_VFX"; //enemy가 사용하는 vfx  
        Object.Instantiate<GameObject>(Resources.Load<GameObject>(enemyHitVFXPath),playerInterface.transform.position,Quaternion.identity);
        int damage;
        GetDefense();
        damage = DamageCalculate(enemyAttack,playerDefense);

        playerInterface.GetDamage(damage);
    }

    /// <summary>
    /// 공격력과 방어력을 넣으면 공격력-방어력을 반올림시켜서 int로 return하는 함수
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="defence"></param>
    /// <returns></returns>
    public int DamageCalculate(float attack, float defence = 0f)
    {
        if (attack < 0) { return 0; }
        if (defence < 0) { return (int)attack; }

        int calculatedDamage = 0;
        float tmpDamage = attack - defence;//반올림을 하기 위해 저장하는 임시변수

        if (tmpDamage - (int)tmpDamage >= 0.5f)
        {//실수부가 0.5이상이면
            calculatedDamage = (int)tmpDamage + 1;//올림
        }
        else
        {//0.5미만이면
            calculatedDamage = (int)tmpDamage;//버림
        }

        if (tmpDamage >= 0) //일반적인 대미지 계산 
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
