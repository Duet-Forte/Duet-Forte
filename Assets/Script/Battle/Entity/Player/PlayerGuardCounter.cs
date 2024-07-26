using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Util.CustomEnum;
using SoundSet;

public class PlayerGuardCounter : MonoBehaviour
{
    public event Action OnGuardCounterEnd;
    private bool isEnteringGuardCounterPhase;
    private SoundSet.PlayerSoundSet playerSoundSet;
    BattlePresenter battlePresenter;
    float playerAttackStat;
    GuardCounterQTE guardCounterQTE;
    JudgeName guardCounterJudge;
    float guardCounterDelay = 5f;
    private void Start()
    {
        guardCounterQTE = Instantiate(Resources.Load<GameObject>("UI/GuardCounterQTE"))
            .GetComponent<GuardCounterQTE>();
        battlePresenter = FindObjectOfType<StageManager>().BattlePresenter;
        
        playerAttackStat= GetComponent<PlayerStatus>().PlayerAttack;

        playerSoundSet= new SoundSet.PlayerSoundSet();
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
            guardCounterDelay = 5f;
            battlePresenter.GuardCounterToEnemy(new Damage(CalcGuardCounterDamage(), guardCounterJudge,new SlashDamage()));
            playerSoundSet.PlayerGuardCounter(gameObject);
            GetComponent<PlayerAnimator>().guardCount();
        }
        if (guardCounterJudge == JudgeName.Miss) { guardCounterDelay = 0f; }
        yield return new WaitForSeconds(guardCounterDelay);
        yield return null;
    }

    private int CalcGuardCounterDamage() {
        return ((int)playerAttackStat * 3) / 10;
    
    }
  

}
