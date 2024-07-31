using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.CustomEnum;

public class TrueDamage : DamageType
{

    public override int GetDamage(float attack, int defense, JudgeName judgeName)//Æ®·ç´ë¹ÌÁö
    {
        if (judgeName == JudgeName.Miss)
        { //RestÆÇÁ¤
            return 0;
        }
        if (judgeName == JudgeName.Perfect)
        {
            return (int)(attack * 1.2f);
        }

        if (judgeName == JudgeName.Great)
        {
            return (int)(attack * 1f);
        }
        if (judgeName == JudgeName.Good)
        {
            return (int)(attack * 0.8f);
        }
        if (judgeName == JudgeName.Bad)
        {
            return (int)(attack * 0.5f);
        }
        return 0;
    }
    public override Util.CustomEnum.DamageType GetDamageType()
    {
        return Util.CustomEnum.DamageType.GuardCounter;
    }

}
