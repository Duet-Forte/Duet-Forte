using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class GuardGaugeUI : InGameUI
{
    [SerializeField]private Image filling;

    public void InitSettings()
    {
        //filling = transform.Find("GuageFilling").GetComponent<Image>();
        Debug.Log(filling);
        filling.fillAmount = 0;
        //SubscribeBeatingUISequence();
    }

    public void ResetGauge()
    {
        filling.DOFillAmount(0, Const.STATUSUI_FADE_SPEED);
    }

    public void UpdateGauge(int currentCombo, int maxGauge)
    {
        float targetGuageAmount = ((float)currentCombo / maxGauge);
        filling.DOFillAmount(targetGuageAmount, Const.STATUSUI_FADE_SPEED);
    }
}
