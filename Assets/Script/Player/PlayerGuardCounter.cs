using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Director;
using Util.CustomEnum;
public class PlayerGuardCounter : MonoBehaviour
{
    public event Action OnGuardCounterEnd;
    private bool isEnteringGuardCounterPhase;

    BattlePresenter battlePresenter;
    float playerAttackStat;
    GuardCounterQTE guardCounterQTE;
    float hitDelay = 0.45f;
    private void Start()
    {
        guardCounterQTE = Instantiate(Resources.Load<GameObject>("UI/GuardCounterQTE"))
            .GetComponent<GuardCounterQTE>();
        battlePresenter = FindObjectOfType<StageManager>().BattlePresenter;

        playerAttackStat = GetComponent<PlayerStatus>().PlayerAttack;
    }
    public void CheckCombo(int currentCombo, int maxGauge)
    {
        if (currentCombo >= maxGauge)
        {
            isEnteringGuardCounterPhase = true;//����ī���� ����
        }
    }

    public IEnumerator EnterGuardCounterPhase()
    {
        if (isEnteringGuardCounterPhase)
        {
            Debug.Log("���� ī����!");
            yield return ProgressGuardCounterPhase(); //���� ī���� �� ����
            isEnteringGuardCounterPhase = false;
            OnGuardCounterEnd?.Invoke();//����ī���� ������ ����
        }
    }


    private IEnumerator ProgressGuardCounterPhase()
    {
        yield return guardCounterQTE.StartQTE(GetComponent<PlayerTurn>().BattlePos); //QTE ���
        if (guardCounterQTE.GetQTEJudge != JudgeName.Miss)
        {
            battlePresenter.GuardCounterToEnemy(CalculateBasicAttackDamage(guardCounterQTE.GetQTEJudge));
            GetComponent<PlayerAnimator>().guardCount();
        }
        yield return new WaitForSeconds(hitDelay);

        yield return new WaitForSeconds(1.5f);
        yield return null;
    }

    float CalculateBasicAttackDamage(JudgeName judgeName)
    {
        if (judgeName == JudgeName.Miss)
        { //Rest����
            return 0;
        }
        if (judgeName == JudgeName.Perfect)
        {
            return playerAttackStat * 2.0f;
        }
        if (judgeName == JudgeName.Great)
        {
            return playerAttackStat * 1.6f;
        }
        if (judgeName == JudgeName.Good)
        {
            return playerAttackStat * 1f;
        }
        return 0;
    }

}