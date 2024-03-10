using UnityEngine;
using Util;

public class StatusUIView : MonoBehaviour, IStageUI
{
    private GameObject[] prefabs; // ���߿� �������� ��� �ִ� ����ƽ �����̳� ���� ����.

    private GuardGaugeUI guardGaugeUI;
    private PlayerHealthPointUI playerHealthPointUI;
    private EnemyHealthPointUI enemyHealthPointUI;
    private PrepareTurnUI prepareTurnUI;

    public void InitSettings(StageManager stageManager, Canvas canvas) // judgeManager�� ���� �÷��̾� ������ ������ �ִ� Ŭ������ ����� ����.
    {
        if(prefabs == null)
        {
            prefabs = new GameObject[3];
            prefabs[0] = Resources.Load<GameObject>(Const.UI_GUARDGAUGE_PATH);
            prefabs[1] = Resources.Load<GameObject>(Const.UI_PLAYERHP_PATH);
            prefabs[2] = Resources.Load<GameObject>(Const.UI_ENEMYHP_PATH);
        }
        guardGaugeUI = Instantiate(prefabs[0], transform)
            .AddComponent<GuardGaugeUI>();
        guardGaugeUI.InitSettings();

        playerHealthPointUI = Instantiate(prefabs[1], transform)
            .AddComponent<PlayerHealthPointUI>();
        playerHealthPointUI.InitSettings(stageManager.JudgeManager.HP);

        enemyHealthPointUI = Instantiate(prefabs[2], transform)
            .AddComponent<EnemyHealthPointUI>();
        enemyHealthPointUI.InitSettings(stageManager.Enemy.HealthPoint);

        BindUIEvents(stageManager);
    }

    private void BindUIEvents(StageManager stageManager)
    {
        BindGuardGaugeUIEvents(stageManager);
        BindPlayerHealthPointUIEvents(stageManager);
        BindEnemyHealthPointUIEvents(stageManager);
    }

    private void BindGuardGaugeUIEvents(StageManager stageManager)
    {
        stageManager.Enemy.OnGuardCounterEnd -= guardGaugeUI.ResetGauge;
        stageManager.Enemy.OnGuardCounterEnd += guardGaugeUI.ResetGauge;
        stageManager.JudgeManager.OnComboChange -= guardGaugeUI.UpdateGauge;
        stageManager.JudgeManager.OnComboChange += guardGaugeUI.UpdateGauge;
    }

    private void BindPlayerHealthPointUIEvents(StageManager stageManager)
    {
        stageManager.Enemy.OnAttack -= playerHealthPointUI.GetDamage;
        stageManager.Enemy.OnAttack += playerHealthPointUI.GetDamage;
    }
    private void BindEnemyHealthPointUIEvents(StageManager stageManager)
    {
        stageManager.Enemy.OnGetDamage -= enemyHealthPointUI.GetDamage;
        stageManager.Enemy.OnGetDamage += enemyHealthPointUI.GetDamage;
    }
}
