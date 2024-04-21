using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class SlashDamage : DamageType
{
    public override Util.CustomEnum.DamageType GetDamageType()
    {
        return Util.CustomEnum.DamageType.Slash;
    }
}
