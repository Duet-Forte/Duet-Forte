using UnityEngine;
using UnityEngine.AI;
using Util.CustomEnum;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField] private RectTransform dialogue;
    [SerializeField] private RectTransform nameWindow;
    [SerializeField] private RectTransform image;

    public void SetPosition(Speaker currentSpeaker)
    {
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

    private void ResetDialogueWindowPosition()
    {
        dialogue.anchorMin = Vector2.zero;
        dialogue.anchorMax = Vector2.one;
        dialogue.offsetMin = new Vector2(340, 35);
        dialogue.offsetMax = new Vector2(-70, -60);
    }
}
