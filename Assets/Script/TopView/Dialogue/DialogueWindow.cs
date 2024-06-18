using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Util.CustomEnum;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField] private RectTransform dialogue;
    [SerializeField] private RectTransform nameWindow;
    [SerializeField] private RectTransform image;
    [SerializeField] private Image arrow;
    [SerializeField] private TMP_Text arraowText;
    private TextMeshProUGUI dialogueText;
    private Tween glowArrow, glowText;
   
    public void SetPosition(Speaker currentSpeaker)
    {
        Debug.Log("대사 대사");
        switch (currentSpeaker)
        {
            case Speaker.player:
                ResetDialogueWindowPosition();

                nameWindow.gameObject.SetActive(true);
                nameWindow.anchorMin = Vector2.up;
                nameWindow.anchorMax = Vector2.up;
                nameWindow.anchoredPosition = new Vector2(150, nameWindow.anchoredPosition.y);

                image.anchorMin = new Vector2(0, 0.5f);
                image.anchorMax = new Vector2(0, 0.5f);
                image.anchoredPosition = new Vector2(160, image.anchoredPosition.y);
                break;

            case Speaker.NPC:
                ResetDialogueWindowPosition();

                nameWindow.gameObject.SetActive(true);
                nameWindow.anchorMin = Vector2.one;
                nameWindow.anchorMax = Vector2.one;
                nameWindow.anchoredPosition = new Vector2(-150, nameWindow.anchoredPosition.y);

                image.anchorMin = new Vector2(1, 0.5f);
                image.anchorMax = new Vector2(1, 0.5f);
                image.anchoredPosition = new Vector2(-160, image.anchoredPosition.y);
                break;

            case Speaker.Empty:
                nameWindow.gameObject.SetActive(false);
                dialogue.anchorMin = Vector2.zero;
                dialogue.anchorMax = Vector2.one;
                dialogue.offsetMin = new Vector2(70, 35);
                dialogue.offsetMax = new Vector2(-70, -35);
                break;
        }
        
    }
    private void OnEnable()
    {
        arrow.color = Color.white;
        arraowText.color = Color.white;
        if(glowArrow == null)
            glowArrow = arrow.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
        if (glowText == null)
            glowText = arraowText.DOFade(0f,1f).SetLoops(-1, LoopType.Yoyo);
    }
    /// <summary>
    /// 텍스트 잔상이 남는 효과를 지우기 위한 함수
    /// </summary>
    public void EraseContent()
    {
        if (dialogueText == null)
            dialogueText = dialogue.GetComponent<TextMeshProUGUI>();
        dialogueText.text = string.Empty;
    }
    private void ResetDialogueWindowPosition()
    {
        dialogue.anchorMin = Vector2.zero;
        dialogue.anchorMax = Vector2.one;
        dialogue.offsetMin = new Vector2(340, 35);
        dialogue.offsetMax = new Vector2(-70, -60);
    }
}
