using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;
using TMPro;

public class EnemyHealthPointUI : InGameUI
{
    private Image damagedFilling;
    private Image filling;
    private bool isProductionProceeding;
    private float enemyMaxHealthPoint;
    private float currentHealthPoint;
    private TMP_Text healthPoint;
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
        healthPoint = transform.Find("HealthPointText").GetComponent<TMP_Text>();
        healthPoint.text = enemyMaxHealthPoint.ToString() + "/"+ enemyMaxHealthPoint.ToString();

    }

    public void GetDamage(int damage)
    {
        currentHealthPoint -= damage;
        healthPoint.text = currentHealthPoint.ToString()+"/"+enemyMaxHealthPoint.ToString();
        float currentHealthPointRatio = Mathf.Clamp01(currentHealthPoint / enemyMaxHealthPoint);
        filling.DOFillAmount(currentHealthPointRatio, Const.STATUSUI_PROCESS_SPEED);
        // 기본적으로 filling이 현재체력에 맞춰 줄어들고, 해당 동작 후 일정 시간이 지나면 damagedFilling이 따라감.
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