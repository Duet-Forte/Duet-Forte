using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GuardCounterTurn : ITurnHandler
{
    StageManager stageManager;
    IEnemy enemy;
    EnemyData enemyData;
    PlayerGuardCounter playerGuardCounter;
    public void InitSettings(StageManager stageManager)
    {
        this.stageManager = stageManager;
        playerGuardCounter = stageManager.PlayerInterface.PlayerGuardCounter;
        enemy = stageManager.Enemy;
        enemyData = enemy.Data;

    }
    public IEnumerator TurnStart()
    {
        //플레이어의 가드카운터 게이지 확인
        
        yield return playerGuardCounter.EnterGuardCounterPhase();
        //가드카운터 게이지가 풀충전이면 가드카운터 진행
        //QTE 활성화
        Debug.Log("가드카운터 턴 시작");
        yield return null;
    }
    

    public IEnumerator TurnEnd()
    {
        //각자 본인 위치로 돌아감.
        stageManager.CurrentTurn = CustomEnum.Turn.ActionEndTurn;
        Debug.Log("가드카운터 턴 종료");
        yield return null;
    }
}
