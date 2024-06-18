using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class GuardGaugeUI : InGameUI
{
    [SerializeField]private Image filling;

    public void InitSettings()
    {
        filling = transform.Find("GuageFilling").GetComponent<Image>();
        Debug.Log(filling);
        filling.fillAmount = 0.5f;
        //SubscribeBeatingUISequence();
    }

    public void ResetGauge()
    {
        filling.DOFillAmount(0, Const.STATUSUI_FADE_SPEED);
    }

    public void UpdateGauge(int currentCombo, int maxGauge)
    {
        float targetGuageAmount = Mathf.Clamp01(((float)currentCombo / maxGauge));
         Debug.Log("가드카운터 filling : "+targetGuageAmount);
        filling.DOFillAmount(targetGuageAmount, Const.STATUSUI_FADE_SPEED);
        Debug.Log(filling.fillAmount);
    }
}
