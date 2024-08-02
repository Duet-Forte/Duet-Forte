using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimpaniOrc : Enemy_Prefab
{ 
    
    private EnemyPattern[] phase2EnemyPattern= new EnemyPattern[3];
    public override void InitSettings(StageManager currentStageManager, Transform playerTransform)
    {
        base.InitSettings(currentStageManager, playerTransform);
        buffManager.AddBuff(new TimpaniDurabilityBuff(battlePresenter, this, 30));
        int[] aPattern = {4, 4, 16, 4, 4, 16};
        int[] bPattern = { 4, 16, 2, 16, 2, 16 };
        int[] cPattern = { 4, 16, 4, 16, 4, 16,4,16 };
        EnemyPattern a = new EnemyPattern(aPattern);
        EnemyPattern b = new EnemyPattern(bPattern);
        EnemyPattern c = new EnemyPattern(cPattern);

        phase2EnemyPattern[0] = a;
        phase2EnemyPattern[1] = b;
        phase2EnemyPattern[2] = c;



    }

    public void SetAttackStat(float percent) { 

        base.enemyAttack=enemyAttack*percent;
    
    }

    public override void ReturnToOriginPos()
    {
        base.ReturnToOriginPos();
        if (currentHP <= (maxHP / 2))
        {
            enemyPattern = phase2EnemyPattern;
        }
        
    }
}
