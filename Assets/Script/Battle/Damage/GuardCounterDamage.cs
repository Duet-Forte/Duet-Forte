using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.CustomEnum;

public class GuardCounterDamage : DamageType
{
    public override int GetDamage(float attack, int defense, JudgeName judgeName)//트루대미지
    {
        if (judgeName == JudgeName.Miss)
        { //Rest판정
            return 0;
        }
        if (judgeName == JudgeName.Perfect)
        {
            return (int)(attack * 1.0f);
        }
        if (judgeName == JudgeName.Great)
        {
            return (int)(attack * 0.8f);
        }
        if (judgeName == JudgeName.Good)
        {
            return (int)(attack * 0.5f);
        }
        if (judgeName == JudgeName.Bad)
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
