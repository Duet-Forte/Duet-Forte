using UnityEngine;
using UnityEngine.Pool;
using Util;

public class DamagePool : IStageUI
{
    private Canvas canvas;
    private IObjectPool<DamageUI> pool;
    private IEnemy enemy;

    public void InitSettings(StageManager stageManager, Canvas canvas)
    {
        enemy = stageManager.Enemy;
        this.canvas = canvas;
        pool = new ObjectPool<DamageUI>(CreateDamageUI, OnGetUI, OnReleaseUI, OnDestroyUI, maxSize: 4);
        BindEvent();
    }

    private DamageUI CreateDamageUI()
    {
        GameObject damageUIPrefab = Resources.Load<GameObject>(Const.UI_DAMAGE);
         DamageUI damageUI = Object
            .Instantiate(damageUIPrefab)
            .GetComponent<DamageUI>();
        damageUI.InitSettings(pool, canvas);
        return damageUI;
    }

    private void OnGetUI(DamageUI damageUI)
    {
        Vector2 spawnPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, enemy.Transform.position);
        damageUI.RectTransform.position = new Vector2 (spawnPoint.x, spawnPoint.y + 300);
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

    public void ShowDamage(int damage) => pool.Get().ParseDamage(damage);
}
