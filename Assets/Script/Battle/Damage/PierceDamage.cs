using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class PierceDamage : DamageType
{
    public override CustomEnum.DamageType GetDamageType()
    {
        return CustomEnum.DamageType.Pierce;
    }
}
