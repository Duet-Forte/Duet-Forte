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
        float tmpDamage = attack - defense;//�ݿø��� �ϱ� ���� �����ϴ� �ӽú���

        if (tmpDamage - (int)tmpDamage >= 0.5f)
        {//�Ǽ��ΰ� 0.5�̻��̸�
            calculatedDamage = (int)tmpDamage + 1;//�ø�
        }
        else
        {//0.5�̸��̸�
            calculatedDamage = (int)tmpDamage;//����
        }

        if (tmpDamage > 0) //�Ϲ����� ����� ��� 
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
        { //Rest����
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
