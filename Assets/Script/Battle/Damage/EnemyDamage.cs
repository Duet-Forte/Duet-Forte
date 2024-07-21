using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.CustomEnum;

public class EnemyDamage : DamageType
{
    public override int GetDamage(float attack, int defense, JudgeName judgeName) //�и� ����
    {
        if (judgeName == JudgeName.Miss) //�и� ����
        { 
            return GetDamage((int)(attack * 1f), (int)(defense * 1f));
        }
        if (judgeName == JudgeName.Perfect) //����Ʈ �и�
        {
            return GetDamage((int)(attack * 0f),(int)(defense * 1f));
        }
        if (judgeName == JudgeName.Great)
        {
            return GetDamage((int)(attack * 0.2f), (int)(defense*0.8f));
        }
        if (judgeName == JudgeName.Good)
        {
            return GetDamage((int)(attack * 0.5f), (int)(defense*0.5f));
        }
        if (judgeName == JudgeName.Bad)
        {
            return GetDamage((int)(attack * 0.8f), (int)(defense*0.2f));
        }
        return 0;
    }
    public override Util.CustomEnum.DamageType GetDamageType()
    {
        return Util.CustomEnum.DamageType.Enemy;
    }
}
