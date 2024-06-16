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
            return (int)(attack * 1f);
        }
        if (judgeName == JudgeName.Perfect) //����Ʈ �и�
        {
            return (int)(attack * 0f);
        }
        if (judgeName == JudgeName.Great)
        {
            return (int)(attack * 0.2f);
        }
        if (judgeName == JudgeName.Good)
        {
            return (int)(attack * 0.5f);
        }
        if (judgeName == JudgeName.Bad)
        {
            return (int)(attack * 0.8f);
        }
        return 0;
    }
    public override Util.CustomEnum.DamageType GetDamageType()
    {
        return Util.CustomEnum.DamageType.Enemy;
    }
}
