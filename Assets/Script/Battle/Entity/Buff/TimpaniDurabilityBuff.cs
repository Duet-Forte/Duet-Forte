using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TimpaniDurabilityBuff : IBuff
{
    private int turnCount;
    private BattlePresenter battlePresenter;
    private TimpaniOrc enemy;
    private int initStack;
    private int stack;
    private int quaterDamage;
    private TMP_Text stackText;
    public TimpaniDurabilityBuff(BattlePresenter battlePresenter, TimpaniOrc enemy, int initStack)
    {
        this.battlePresenter = battlePresenter;
        this.enemy = enemy;
        quaterDamage = enemy.MaxHP/4; 
        enemy.OnHit += DecreaseStack;
        this.initStack = initStack;
        stack = initStack;
        UISettings(enemy.gameObject);
    }
    public void UISettings(GameObject parent) {
        GameObject UI=GameObject.Instantiate(Resources.Load<GameObject>("UI/Buff/TimpaniDurability"), parent.transform);
        UI.transform.localPosition = new Vector2(0, 5f);
        stackText=UI.GetComponentInChildren<TMP_Text>();
        stackText.text = stack.ToString();
    }
    public void PlusStack(int plus) {
        stack += plus;
    }
    public int GetStack() {
        return stack;
    }
    public void UpdateStack() {
        if (stack <= 0) {
            stack = initStack;
            stackText.text = stack.ToString();
            enemy.OnHit += DecreaseStack;
        }
    }

    private void DecreaseStack() {
        if (stack > 0)
        {
            stack--;
        }
        stackText.text = stack.ToString();
        if (stack <= 0) {
            enemy.OnHit -= DecreaseStack;
            
            Execute();
        }
    }
    private void Execute() {
        //스택이 터지면 전체 체력의 25%감소
        battlePresenter.PlayerBasicAttackToEnemy(new Damage(quaterDamage,new SlashDamage()));
        //공격력 30퍼 증가
        enemy.SetAttackStat(1.3f);
        //스택이 터진 턴에는 더 이상 스택 감소 X
        stackText.text = " ";
        
    }
    public bool CheckStack() {
        return false;
    
    }
    
}
