using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Util.CustomEnum;
public class PlayerGuardCounter : MonoBehaviour
{
    public event Action OnGuardCounterEnd;
    private bool isEnteringGuardCounterPhase;

    BattlePresenter battlePresenter;
    float playerAttackStat;
    GuardCounterQTE guardCounterQTE;
    JudgeName guardCounterJudge;
    float hitDelay = 0.45f;
    private void Start()
    {
        guardCounterQTE = Instantiate(Resources.Load<GameObject>("UI/GuardCounterQTE"))
            .GetComponent<GuardCounterQTE>();
        battlePresenter = FindObjectOfType<StageManager>().BattlePresenter;
        
        playerAttackStat= GetComponent<PlayerStatus>().PlayerAttack;
    }
    public void CheckCombo(int currentCombo, int maxGauge)
    {
        if (currentCombo >= maxGauge)
        {
            isEnteringGuardCounterPhase = true;//가드카운터 실행
        }
    }
    
    public IEnumerator EnterGuardCounterPhase()
    {
        guardCounterJudge = JudgeName.Miss; //null값대신 초기화
        if (isEnteringGuardCounterPhase)
        {
            Debug.Log("가드 카운터!");
            yield return ProgressGuardCounterPhase(); //가드 카운터 시 진행
            isEnteringGuardCounterPhase = false;
            OnGuardCounterEnd?.Invoke();//가드카운터 게이지 리셋
        }
    }

    
    private IEnumerator ProgressGuardCounterPhase()
    {
        yield return guardCounterQTE.StartQTE(GetComponent<PlayerTurn>().BattlePos); //QTE 재생
        guardCounterJudge = guardCounterQTE.GetQTEJudge;
        if (guardCounterJudge != JudgeName.Miss)
        {
            battlePresenter.GuardCounterToEnemy(new Damage(playerAttackStat, guardCounterJudge,new GuardCounterDamage()));
            GetComponent<PlayerAnimator>().guardCount();
        }
        yield return new WaitForSeconds(hitDelay);

        yield return new WaitForSeconds(4.5f);
        yield return null;
    }

  

}
