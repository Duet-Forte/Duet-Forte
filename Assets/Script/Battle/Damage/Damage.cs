using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.CustomEnum;

public class Damage 
{
    public Damage(float attack,JudgeName judgeName,DamageType damageType ) { 
    this.attack = attack;
    this.judgeName = judgeName;
    this.damageType = damageType;
    }
    public Damage(float attack, DamageType damageType)
    {
        this.attack = attack;
        this.judgeName = JudgeName.Perfect;
        this.damageType = damageType;
    }
    public Damage(float attack)
    {
        this.attack = attack;
        this.judgeName = JudgeName.Perfect;
        this.damageType = new GuardCounterDamage();
    }
    

    private float attack;
    private int calculatedAttack;
    private JudgeName judgeName;
    private DamageType damageType;
    public JudgeName JudgeName { get { return judgeName; } }
    public void CalculateDamage(int defense) {

        calculatedAttack= damageType.GetDamage(attack, defense);
    }
    public void CalculateDamageWithJudge(int defense)
    {
        calculatedAttack= damageType.GetDamage(attack, defense,judgeName);
    }
    public int GetCalculatedDamage() { 
        return calculatedAttack;
    }
    public Util.CustomEnum.DamageType GetDamageType() {
        return damageType.GetDamageType();
    }
    
}
