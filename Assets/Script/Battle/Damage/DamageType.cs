using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public abstract class DamageType
{
    public abstract Util.CustomEnum.DamageType GetDamageType();
    public virtual int GetDamage(float attack, int defense) {
        if (attack < 0) { return 0; }
        if (defense < 0) { return (int)attack; }

        int calculatedDamage = 0;
        float tmpDamage = attack - defense;//반올림을 하기 위해 저장하는 임시변수

        if (tmpDamage - (int)tmpDamage >= 0.5f)
        {//실수부가 0.5이상이면
            calculatedDamage = (int)tmpDamage + 1;//올림
        }
        else
        {//0.5미만이면
            calculatedDamage = (int)tmpDamage;//버림
        }

        if (tmpDamage > 0) //일반적인 대미지 계산 
        {
            return calculatedDamage;
        }
        if (tmpDamage <= 0)
        {
            return 0;
        }
        return 0;
    }
    public virtual int GetDamage(float attack, int defense, Util.CustomEnum.JudgeName judgeName) {
        if (judgeName == Util.CustomEnum.JudgeName.Miss)
        { //Rest판정
            return 0;
        }
        if (judgeName == Util.CustomEnum.JudgeName.Perfect)
        {
            return GetDamage(attack * 1.0f,defense);
        }
        if (judgeName == Util.CustomEnum.JudgeName.Great)
        {
            return GetDamage(attack * 0.8f, defense);
        }
        if (judgeName == Util.CustomEnum.JudgeName.Good)
        {
            return GetDamage(attack * 0.5f, defense);
        }
        if (judgeName == Util.CustomEnum.JudgeName.Bad)
        {
            return GetDamage(attack * 0.2f, defense);
        }
        return 0;
    }
}
