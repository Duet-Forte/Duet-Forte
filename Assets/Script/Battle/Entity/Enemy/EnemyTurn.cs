using System.Collections;
using UnityEngine;
using Util.CustomEnum;
public class EnemyTurn : ITurnHandler
{
    private StageManager stageManager;
    private IEnemy enemy;
    private EnemyData enemyData;
    private int[] nextPattern;
    
    public void InitSettings(StageManager stageManager)
    {
        this.stageManager = stageManager;
        enemy = stageManager.Enemy;
        enemyData = enemy.Data;
        
    }
    public IEnumerator TurnStart()
    {
        Debug.Log("에너미 턴 시작");
        nextPattern = enemy.EnemyPattern[Random.Range(0, enemy.EnemyPattern.Length)].patternArray;
        yield return enemy.DisplayPattern(nextPattern);                             // 패턴 보여주기
        yield return enemy.Attack();                                                // 실제 공격
    }

    public IEnumerator TurnEnd()
    {
        stageManager.CurrentTurn = Turn.GuardCounterTurn;                
        Debug.Log("에너미 턴 종료");
        yield return null;
    }
}
