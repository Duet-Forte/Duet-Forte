using System;
using System.Collections;
using UnityEngine;

public interface IEnemy
{
    public Transform Transform { get; }
    public bool IsNoteChecked { get;}
    public double JudgeStartTime { get;}
    public double JudgeEndTime { get;}
    public EnemyData Data { get;}
    public EnemyPattern[] EnemyPattern { get; }
    public int HealthPoint { get; }
    public string EnemyName { get; }
    public Vector2 Defense { get; }
    public void InitSettings(StageManager stageManager, Transform playerTransform);
    public IEnumerator DisplayPattern(int[] pattern);
    public IEnumerator Attack();
    public event Action OnFramePass;
    public event Action OnGuardCounterEnd;
    public event Action<int> OnAttack;
    public event Action<int> OnGetDamage;
    
    public void BindPattern(EnemyPattern[] enemyPattern);
    public void CheckCombo(int currentCombo, int maxGauge);
    public void HandleParryJudge(Judge judge, int damage);
    public void GiveDamage();
    /// <summary>
    /// bool 값은 true이면 베기(A공격), false이면 찌르기(B공격)
    /// 방어도에 따라 실제 받는 대미지로 계산되는 함수가 있음.
    /// </summary>
    /// <param name="playerAttack"></param>
    /// <param name="isSlash">true는 베기-A,false는 찌르기-B</param>
    public void GetDamage(int playerAttack,bool isSlash);
    public void GetDamage(int playerAttack);
    public void StopActions();

    public void ReturnToOriginPos();
    public void DashToBattlePos();

    public Vector2 GetDefense();
}
