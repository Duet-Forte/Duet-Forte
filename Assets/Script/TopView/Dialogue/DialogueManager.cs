using Cysharp.Threading.Tasks;
using Febucci.UI;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.CustomEnum;

public class DialogueManager
{
    private static DialogueManager instance;

    private GameObject window;
    private DialogueWindow dialogueWindow;
    private Image characterSprite;
    private TextMeshProUGUI talkerName;
    private TextMeshProUGUI contentText;
    private TypewriterByCharacter typewriter;
    private Speaker currentSpeaker;
    private CancellationTokenSource cancel;
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DialogueManager();
            return instance;
        }
    }
    public async UniTask Talk(string speakerName)
    {
        if (window == null)
        {
            window = Object.Instantiate(Resources.Load<GameObject>("TopView/Dialogue/Window"));
            TextMeshProUGUI[] texts = window.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts[0].name == "Dialogue")
            {
                contentText = texts[0];
                talkerName = texts[1];
            }
            else
            {
                contentText = texts[1];
                talkerName = texts[0];
            }
            typewriter = contentText.GetComponent<TypewriterByCharacter>();
            characterSprite = window.transform.GetChild(0).Find("Image").GetComponent<Image>();
            dialogueWindow = window.GetComponent<DialogueWindow>();
        }
        cancel = new CancellationTokenSource();
        window.SetActive(true);
        Dialogue dialogue = DataBase.Instance.Dialogue.GetDialogue(speakerName);
        await WaitKeyInput(dialogue, speakerName);
    }

    private async UniTask WaitKeyInput(Dialogue dialogue, string interactorName)
    {
        for (int i = 0; i < dialogue.Lines.Length; i++)
        {
            dialogue.Speaker = dialogue.Speakers[i];
            if (dialogue.Events[i] != null)
            {
                if(dialogue.Events[i].PlayEvent(dialogue, interactorName))
                    continue;
                else
                {
                    window.SetActive(false);
                    await UniTask.WaitUntil(IsKeyTriggered, cancellationToken: cancel.Token);
                    window.SetActive(true);
                    continue;
                }
            }
            characterSprite.enabled = true;
            typewriter.ShowText(dialogue.Lines[i]);
            if (dialogue.Speaker != null)
            {
                if (dialogue.Speaker.Split('/')[0] == "Empty")
                {
                    talkerName.text = string.Empty;
                    currentSpeaker = Speaker.Empty;
                }
                else
                {
                    talkerName.text = Util.Method.ChangeToKRName(dialogue.Speaker.Split('/')[0]);
                }

                if (dialogue.Speaker.Split('/')[0] == "Zio")
                {
                    currentSpeaker = Speaker.player;
                }
                else
                    currentSpeaker = Speaker.NPC;

                if (dialogue.Sprite != null)
                    characterSprite.sprite = dialogue.Sprite;
                else
                    characterSprite.enabled = false;
            }

            dialogueWindow.SetPosition(currentSpeaker);
            await UniTask.Delay(500, cancellationToken: cancel.Token);
            await UniTask.WaitUntil(IsKeyTriggered, cancellationToken: cancel.Token);
            typewriter.SkipTypewriter();
            await UniTask.WaitUntil(IsKeyTriggered, cancellationToken: cancel.Token);
        }
        dialogueWindow.EraseContent();
        window.SetActive(false);
    }
    private bool IsKeyTriggered()
    {
        return SceneManager.Instance.InputController.IsKeyTriggered(PlayerAction.Interact);
    }

    public void SkipDialogue()
    {
        cancel.Cancel();
        window.SetActive(false);
    }
}