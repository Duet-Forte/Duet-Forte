using UnityEngine;
using UnityEngine.Pool;
using Util;

public class DamagePool : IStageUI
{
    private IObjectPool<DamageUI> pool;
    private IEnemy enemy;

    public void InitSettings(StageManager stageManager, Canvas canvas)
    {
        enemy = stageManager.Enemy;
        pool = new ObjectPool<DamageUI>(CreateDamageUI, OnGetUI, OnReleaseUI, OnDestroyUI, maxSize: 10);
        BindEvent();
    }

    private DamageUI CreateDamageUI()
    {
        GameObject damageUIPrefab = Resources.Load<GameObject>("UI/Damage/DamageText");
         DamageUI damageUI = Object
            .Instantiate(damageUIPrefab)
            .GetComponentInChildren<DamageUI>();
        damageUI.InitSettings(pool);
        return damageUI;
    }

    private void OnGetUI(DamageUI damageUI)
    {
        Vector2 spawnPoint = enemy.Transform.position;
        float randomOffsetX = Random.RandomRange(-1f, 1f);
        float randomOffsetY = Random.RandomRange(-1f, 1f);
        damageUI.Position.position = new Vector2 (spawnPoint.x+randomOffsetX, spawnPoint.y+randomOffsetY);
        damageUI.MoveUI();
    }

    private void OnReleaseUI(DamageUI damageUI)
    {
        damageUI.ResetSettings();
    }

    private void OnDestroyUI(DamageUI damageUI)
    {
        if (damageUI != null)
            Object.Destroy(damageUI.gameObject);
    }

    private void BindEvent()
    {
        enemy.OnGetDamage -= ShowDamage;
        enemy.OnGetDamage += ShowDamage;
    }

    public void ShowDamage(Damage damage) => pool.Get().ParseDamage(damage);//공격유형 추가하기
}
