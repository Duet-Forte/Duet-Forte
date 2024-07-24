using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimpaniDurabilityBuff : IBuff
{
    private int turnCount;
    private BattlePresenter battlePresenter;
    private IEnemy enemy;
    private int initStack;
    private int stack;
    private int quaterDamage;
    public TimpaniDurabilityBuff(BattlePresenter battlePresenter, IEnemy enemy, int initStack)
    {
        this.battlePresenter = battlePresenter;
        this.enemy = enemy;
        quaterDamage = enemy.MaxHP/4;
        enemy.OnHit += DecreaseStack;
        this.initStack = initStack;
        stack = initStack;
    }

    public void PlusStack(int plus) {
        stack += plus;
    }
    public int GetStack() {
        return stack;
    }
    public void UpdateStack() {

    }

    private void DecreaseStack() { 
        stack--;
        if (stack <= 0) {
            enemy.OnHit -= DecreaseStack;
            Execute();
        }
    }
    private void Execute() {
        //���� �ִ� ü�� 25% ����
        battlePresenter.PlayerBasicAttackToEnemy(new Damage(quaterDamage,new SlashDamage()));
        //���ݷ� ����
        //stack initStack���� �ʱ�ȭ
        stack = initStack;
        enemy.OnHit += DecreaseStack;
    }
    public bool CheckStack() {
        return false;
    
    }
    
}
