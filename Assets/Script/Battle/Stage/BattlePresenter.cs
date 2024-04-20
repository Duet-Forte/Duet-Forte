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
    BattleDirector battleDirector=new BattleDirector();
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
    public void PlayerBasicAttackToEnemy(Damage damage) {
        GetDefense();

        if (damage.GetDamageType() == Util.CustomEnum.DamageType.Slash)
        {
            damage.CalculateDamageWithJudge((int)enemySlashDefense);
            enemy.GetDamage(damage);

            if(damage.GetCalculatedDamage()>0)hitParticle.Generate_Player_Hit_Slash(enemy.Transform);
        }
        else
        {
            damage.CalculateDamageWithJudge((int)enemyPierceDefense);
            enemy.GetDamage(damage);
            if (damage.GetCalculatedDamage() > 0) hitParticle.Generate_Player_Hit_Pierce(enemy.Transform);
        }
      
        
    }
    public void PlayerSkillToEnemy(Damage damage) {

        GetDefense();
        
        if (damage.GetDamageType() == Util.CustomEnum.DamageType.Slash)
        {
            damage.CalculateDamage((int)enemySlashDefense);
            enemy.GetDamage(damage);
            CameraShake(0.5f,1f,100,30);
            if (damage.GetCalculatedDamage() > 0) hitParticle.Generate_Player_Hit_Slash(enemy.Transform);
        }
        else
        {
            damage.CalculateDamage((int)enemyPierceDefense);
            enemy.GetDamage(damage);
            CameraShake(0.5f, 1f, 100, 30);
            if (damage.GetCalculatedDamage() > 0) hitParticle.Generate_Player_Hit_Pierce(enemy.Transform);
        }

    }

    public void EnemyToPlayer(Damage enemyAttack) {
        
        string enemyHitVFXPath = Const.ENEMY_VFX_PATH + enemyName+"/"+enemyName+"_BasicHit_VFX"; //enemy�� ����ϴ� vfx  
        Object.Instantiate<GameObject>(Resources.Load<GameObject>(enemyHitVFXPath),playerInterface.transform.position,Quaternion.identity);
        GetDefense();
        enemyAttack.CalculateDamage((int)playerDefense);
        CameraShake(1f,1f,100,50);

        playerInterface.GetDamage(enemyAttack);
    }

    public void GuardCounterToEnemy(Damage damage) {
        damage.CalculateDamageWithJudge(0);//Ʈ������
        StartCoroutine(GuardCounter(damage));
        Object.Instantiate<GameObject>(Resources.Load<GameObject>("VFX/VFX_Prefab/Combat/Player/Hit/Player_CounterAttack_GuardCounter_Hit_VFX"),enemy.Transform.position, Quaternion.identity);

    }
    private void GuardCounterDamage() { 
    
    
    }
    private void CameraShake(float duration, float strength, int vibrato, int randomness) {
        battleDirector.CameraShake(duration, strength, vibrato, randomness);
    }
    IEnumerator GuardCounter(Damage damage) {
        
        enemy.GetDamage(damage);
        yield return new WaitForSeconds(0.45f);
        for (int guardCounterCount = 0; guardCounterCount < 10; guardCounterCount++) {
            enemy.GetDamage(damage);
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
