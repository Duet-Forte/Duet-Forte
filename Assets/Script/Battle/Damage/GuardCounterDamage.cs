using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GuardCounterDamage : DamageType
{
    public override int GetDamage(float attack, int defense, Util.CustomEnum.JudgeName judgeName)//Ʈ������
    {
        if (judgeName == Util.CustomEnum.JudgeName.Miss)
        { //Rest����
            return 0;
        }
        if (judgeName == Util.CustomEnum.JudgeName.Perfect)
        {
            return (int)(attack * 1.0f);
        }
        if (judgeName == Util.CustomEnum.JudgeName.Great)
        {
            return (int)(attack * 0.8f);
        }
        if (judgeName == Util.CustomEnum.JudgeName.Good)
        {
            return (int)(attack * 0.5f);
        }
        if (judgeName == Util.CustomEnum.JudgeName.Bad)
        {
            return (int)(attack * 0.2f);
        }
        return 0;
    }
    public override Util.CustomEnum.DamageType GetDamageType()
    {
        return Util.CustomEnum.DamageType.GuardCounter;
    }

}
