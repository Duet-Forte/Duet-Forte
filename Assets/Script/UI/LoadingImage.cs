
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    [SerializeField] private Image loadingFill;

    public void InitSettings()
    {
        Canvas canvas = GetComponent<Canvas>();
        GetComponentInChildren<ScreenSpaceCameraUI>().InitSettings(canvas);
        loadingFill.fillAmount = 0;
    }

    public void SetFillAmount(float percentage)
    {
        loadingFill.fillAmount = percentage;
    }
}
