using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GuardCounterDamage : DamageType
{
    public override int GetDamage(float attack, int defense, CustomEnum.JudgeName judgeName)//트루대미지
    {
        if (judgeName == CustomEnum.JudgeName.Miss)
        { //Rest판정
            return 0;
        }
        if (judgeName == CustomEnum.JudgeName.Perfect)
        {
            return (int)(attack * 1.0f);
        }
        if (judgeName == CustomEnum.JudgeName.Great)
        {
            return (int)(attack * 0.8f);
        }
        if (judgeName == CustomEnum.JudgeName.Good)
        {
            return (int)(attack * 0.5f);
        }
        if (judgeName == CustomEnum.JudgeName.Bad)
        {
            return (int)(attack * 0.2f);
        }
        return 0;
    }
    public override CustomEnum.DamageType GetDamageType()
    {
        return CustomEnum.DamageType.GuardCounter;
    }

}
