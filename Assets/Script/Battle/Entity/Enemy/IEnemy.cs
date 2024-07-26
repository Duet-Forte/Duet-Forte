using System;
using System.Collections;
using UnityEngine;

public interface IEnemy : IBuffState
{
    public Transform Transform { get; }
    public bool IsNoteChecked { get;}
    public double JudgeStartTime { get;}
    public double JudgeEndTime { get;}
    public EnemyData Data { get;}
    public EnemyPattern[] EnemyPattern { get; }
    public int CurrentHP { get; }
    public int MaxHP { get; }
    public string EnemyName { get; }
    public Vector2 Defense { get; }
    public int Exp { get; }
    public void InitSettings(StageManager stageManager, Transform playerTransform);
    public IEnumerator DisplayPattern(int[] pattern);
    public IEnumerator Attack();
    public event Action OnFramePass;
    public event Action OnGuardCounterEnd;
    public event Action<Damage> OnAttack;
    public event Action<Damage> OnGetDamage;
    public event Action OnHit;
    public event Action OnTurnEnd;
    
    public void BindPattern(EnemyPattern[] enemyPattern);
    public void CheckCombo(int currentCombo, int maxGauge);
    public void HandleParryJudge(Judge judge);
    public void GiveDamage(Judge judge);
    /// <summary>
    /// bool ���� true�̸� ����(A����), false�̸� ���(B����)
    /// ���� ���� ���� �޴� ������� ���Ǵ� �Լ��� ����.
    /// </summary>
    /// <param name="playerAttack"></param>
    /// <param name="isSlash">true�� ����-A,false�� ���-B</param>

    public void GetDamage(Damage damage);
    public void StopActions();

    public void ReturnToOriginPos();
    public void DashToBattlePos();

    public Vector2 GetDefense();
}
