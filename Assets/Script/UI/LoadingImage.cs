
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    [SerializeField] private Image loadingFill;

    public void InitSettings()
    {
        Canvas canvas = GetComponent<Canvas>();
        GetComponentInChildren<ScreenSpaceCameraUI>().InitSettings(canvas);
    }

    public void SetFillAmount(float percentage)
    {
        loadingFill.material.SetFloat("Gauge", 0.9f - percentage);
    }
}
