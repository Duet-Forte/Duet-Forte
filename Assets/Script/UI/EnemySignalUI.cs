using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Director;

public class EnemySignalUI : MonoBehaviour
{
    SignalIcon[] defenseIcons=new SignalIcon[7];
    SignalIcon[] attackIcons= new SignalIcon[7];
    Color blockedAttackColor = new Color(137, 137, 137);
    Color originAttackColor = new Color(255, 255, 255);

    int attackIconCounter = 0;
    int defenseIconCounter = 0;

    BattleDirector battleDirector;

    public void InitSettings() {
        
    GameObject attackIconsAsGameObject=transform.GetChild(0).gameObject;
    GameObject defenseIconsAsGameObject = transform.GetChild(1).gameObject;
         attackIcons= attackIconsAsGameObject.GetComponentsInChildren<SignalIcon>();
        defenseIcons = defenseIconsAsGameObject.GetComponentsInChildren<SignalIcon>();
        DisableAllIcon();

        battleDirector = new BattleDirector();
    }
    
    public void AttackActive() {
        
        attackIcons[attackIconCounter].gameObject.SetActive(true);
        attackIconCounter++;
    }
    public void DefenseActive(Judge judge) {

        if (judge.Color == Color.black)
        {//miss상황일때
            attackIcons[defenseIconCounter].AttackEffect();
            defenseIconCounter++;
        }
        else
        {
            defenseIcons[defenseIconCounter].gameObject.SetActive(true);
            defenseIcons[defenseIconCounter].gameObject.GetComponent<Image>().color = judge.Color;
            attackIcons[defenseIconCounter].gameObject.GetComponent<Image>().color = blockedAttackColor;
            defenseIconCounter++;
        }

    }
    public void ResetIcon() {

        ResetColor();
        DisableAllIcon();
        attackIconCounter = 0;
        defenseIconCounter = 0;

    }

    void DisableAllIcon() {

        for (int i = 0; i < defenseIcons.Length; i++) {
            defenseIcons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < attackIcons.Length; i++)
        {
            attackIcons[i].gameObject.SetActive(false);
        }

    }
    void ResetColor() {
        for (int i = 0; i < defenseIcons.Length; i++)
        {
            defenseIcons[i].gameObject.GetComponent<Image>().color = originAttackColor;
        }
        for (int i = 0; i < attackIcons.Length; i++)
        {
            attackIcons[i].gameObject.GetComponent<Image>().color = originAttackColor;
        }
    }
   
}
