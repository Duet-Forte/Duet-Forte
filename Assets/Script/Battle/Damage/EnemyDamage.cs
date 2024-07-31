using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.CustomEnum;

public class EnemyDamage : DamageType
{
    public override int GetDamage(float attack, int defense, JudgeName judgeName) //패링 판정
    {
        if (judgeName == JudgeName.Miss) //패링 실패
        { 
            return GetDamage((int)(attack * 1f), (int)(defense * 1f));
        }
        if (judgeName == JudgeName.Perfect) //퍼펙트 패링
        {
            return GetDamage((int)(attack * 0f),(int)(defense * 1f));
        }
        if (judgeName == JudgeName.Great)
        {
            return GetDamage((int)(attack * 0.4f), (int)(defense*1f));
        }
        if (judgeName == JudgeName.Good)
        {
            return GetDamage((int)(attack * 0.6f), (int)(defense*1f));
        }
        if (judgeName == JudgeName.Bad)
        {
            return GetDamage((int)(attack * 0.8f), (int)(defense*1f));
        }
        return 0;
    }
    public override Util.CustomEnum.DamageType GetDamageType()
    {
        return Util.CustomEnum.DamageType.Enemy;
    }
}
