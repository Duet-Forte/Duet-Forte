using UnityEngine;
using Util;

public class StatusUIView : MonoBehaviour, IStageUI
{
    private GameObject[] prefabs; // 나중에 프리팹을 들고 있는 스태틱 컨테이너 제작 예정.

    private GuardGaugeUI guardGaugeUI;
    private PlayerHealthPointUI playerHealthPointUI;
    private PlayerHealthPointUI enemyHealthPointUI;
    

    public void InitSettings(StageManager stageManager, Canvas canvas) // judgeManager은 추후 플레이어 스탯을 가지고 있는 클래스로 대신할 예정.
    {
        GetComponent<ScreenSpaceCameraUI>().InitSettings();
        if (prefabs == null)
        {
            prefabs = new GameObject[2];
            prefabs[0] = Resources.Load<GameObject>(Const.UI_PLAYERHP_PATH);
            prefabs[1] = Resources.Load<GameObject>(Const.UI_ENEMYHP_PATH);
        }
    

        playerHealthPointUI = Instantiate(prefabs[0], transform)
            .AddComponent<PlayerHealthPointUI>();
        playerHealthPointUI.InitSettings(stageManager.JudgeManager.HP);
        guardGaugeUI = prefabs[0].GetComponent<GuardGaugeUI>();
        guardGaugeUI.InitSettings();

        enemyHealthPointUI = Instantiate(prefabs[1], transform)
            .AddComponent<PlayerHealthPointUI>();
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
        stageManager.PlayerInterface.OnGetDamage -= playerHealthPointUI.GetDamage;
        stageManager.PlayerInterface.OnGetDamage += playerHealthPointUI.GetDamage;
    }
    private void BindEnemyHealthPointUIEvents(StageManager stageManager)
    {
        stageManager.Enemy.OnGetDamage -= enemyHealthPointUI.GetDamage;
        stageManager.Enemy.OnGetDamage += enemyHealthPointUI.GetDamage;
    }
}
