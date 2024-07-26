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
        Debug.Log("���ʹ� �� ����");
        nextPattern = enemy.EnemyPattern[Random.Range(0, enemy.EnemyPattern.Length)].patternArray;
        yield return enemy.DisplayPattern(nextPattern);                             // ���� �����ֱ�
        yield return enemy.Attack();                                                // ���� ����
    }

    public IEnumerator TurnEnd()
    {
        stageManager.CurrentTurn = Turn.GuardCounterTurn;                
        Debug.Log("���ʹ� �� ����");
        yield return null;
    }
}
