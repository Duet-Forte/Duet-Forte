using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class EnemyHealthPointUI : InGameUI
{
    private Image damagedFilling;
    private Image filling;
    private bool isProductionProceeding;
    private float enemyMaxHealthPoint;
    private float currentHealthPoint;

    private Tween productionTween;
    public void InitSettings(int enemyMaxHealthPoint)
    {
        damagedFilling = transform.Find("DamagedHP").GetComponent<Image>();
        filling = transform.Find("Filling").GetComponent<Image>();
        this.enemyMaxHealthPoint = enemyMaxHealthPoint;
        filling.fillAmount = 0;
        damagedFilling.fillAmount = 0;
        filling.DOFillAmount(Const.STATUSUI_MAX_HP_RATIO, Const.STATUSUI_PROCESS_SPEED);
        damagedFilling.DOFillAmount(Const.STATUSUI_MAX_HP_RATIO, Const.STATUSUI_PROCESS_SPEED);
        currentHealthPoint = enemyMaxHealthPoint;
    }

    public void GetDamage(int damage)
    {
        currentHealthPoint -= damage;
        float currentHealthPointRatio = Mathf.Clamp01(currentHealthPoint / enemyMaxHealthPoint);
        filling.DOFillAmount(currentHealthPointRatio, Const.STATUSUI_PROCESS_SPEED);
        // �⺻������ filling�� ����ü�¿� ���� �پ���, �ش� ���� �� ���� �ð��� ������ damagedFilling�� ����.
        if (isProductionProceeding)
        {
            return;
        }
        productionTween?.Kill();
        productionTween = DOVirtual.DelayedCall(Const.STATUSUI_WAIT_TIME,
            () =>
            {
                isProductionProceeding = true;
                damagedFilling.DOFillAmount(currentHealthPointRatio, Const.STATUSUI_PROCESS_SPEED)
                .OnComplete(() => isProductionProceeding = false);
            });
    }
}