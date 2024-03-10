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
        guardCounterSlash.GenerateGuardCounterSlash(gameObject);    
        //Vector2 targetPosition = (playerTransform.position + transform.position) / 2;//�÷��̾�� ���� �߰� ����
        //qteEffect.StartQTE(targetPosition); QTE ���
        yield return null;
    }
}
