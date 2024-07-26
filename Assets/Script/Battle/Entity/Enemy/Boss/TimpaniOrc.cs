using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimpaniOrc : Enemy_Prefab
{

    public override void InitSettings(StageManager currentStageManager, Transform playerTransform)
    {
        base.InitSettings(currentStageManager, playerTransform);
        buffManager.AddBuff(new TimpaniDurabilityBuff(battlePresenter, this, 30));
    }

    public void SetAttackStat(float percent) { 

        base.enemyAttack=enemyAttack*percent;
    
    }

}
