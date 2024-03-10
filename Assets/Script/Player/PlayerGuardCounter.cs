using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Director;
public class PlayerGuardCounter : MonoBehaviour
{
    public event Action OnGuardCounterEnd;
    private bool isEnteringGuardCounterPhase;
    SpacialAttack guardCounterSlash;

    QTE guardCounterQTE;

    private void Start()
    {
        guardCounterSlash = new SpacialAttack();
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
        guardCounterSlash.GenerateGuardCounterSlash(gameObject);    
        //Vector2 targetPosition = (playerTransform.position + transform.position) / 2;//플레이어와 적의 중간 지점
        //qteEffect.StartQTE(targetPosition); QTE 재생
        yield return null;
    }
}
