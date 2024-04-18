using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SelectPopUp : MonoBehaviour
{
    [SerializeField] private Button applyButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI guideText;

    private Vector2 defaultScale;
    public void InitSettings()
    {
        defaultScale = new Vector2(0.1f, 0.1f);
        gameObject.SetActive(false);
        applyButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        applyButton.onClick.AddListener(ClosePopup);
        cancelButton.onClick.AddListener(ClosePopup);
    }
    public void OnEnable()
    {
        transform.localScale = defaultScale;
        transform.DOScale(Vector2.one, 0.5f);
    }

    public void AddApplyClickEvent(UnityAction applyEvent)
    {
        applyButton.onClick.AddListener(applyEvent);
    }

    public void AddCancelClickEvent(UnityAction cancelEvent)
    {
        cancelButton.onClick.AddListener(cancelEvent);
    }

    public void ChangeGuideText(string guideText)
    {
        this.guideText.text = guideText;
    }
    private void ClosePopup()
    {
        transform.DOScale(defaultScale, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
