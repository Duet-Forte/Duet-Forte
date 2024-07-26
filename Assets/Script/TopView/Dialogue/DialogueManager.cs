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
    private bool isTalking = false;
    public bool IsTalking { get => isTalking; }
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DialogueManager();
            return instance;
        }
    }
    public async UniTask Talk(string interactorName)
    {
        isTalking = true;
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
        Dialogue dialogue = DataBase.Dialogue.GetDialogue(interactorName);
        SkipDialogue(dialogue, interactorName).Forget();
        await WaitKeyInput(dialogue, interactorName);
        isTalking = false;
    }

    public void SkipEvent(Dialogue dialogue, string interactorName, bool isCutscenePlaying = false)
    {
        if(isTalking || isCutscenePlaying)
        {
            for (int i = 0; i < dialogue.Lines.Length; i++)
            {
                DialogueEventHandler dialogueEvent = dialogue.Events[i];
                if (dialogueEvent == null)
                    continue;
                if (!(dialogueEvent.isDone || dialogueEvent.isSkippable))
                {
                    dialogueEvent.PlayEvent(dialogue, interactorName);
                }
            }
            isTalking = false;
        }
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
            await UniTask.WhenAny(UniTask.WaitUntil(IsKeyTriggered, cancellationToken: cancel.Token), UniTask.WaitUntil(IsTypeEnded));
            typewriter.SkipTypewriter();
            await UniTask.WaitUntil(IsKeyTriggered, cancellationToken: cancel.Token);
        }
        dialogueWindow.EraseContent();
        window.SetActive(false);
    }
    private bool IsKeyTriggered()
    {
        return GameManager.InputController.IsKeyTriggered(PlayerAction.Interact);
    }
    private bool IsSkipTriggered()
    {
        return GameManager.InputController.IsKeyTriggered(PlayerAction.Skip);
    }
    private async UniTask SkipDialogue(Dialogue dialogue, string interactorName)
    {
        if (GameManager.CutsceneManager.isPlaying)
            return;
        await UniTask.WaitUntil(IsSkipTriggered);
        EndDialogue();
        SkipEvent(dialogue, interactorName);
    }
    private bool IsTypeEnded()
    {
        return !typewriter.isShowingText;
    }
    public void EndDialogue()
    {
        cancel.Cancel();
        dialogueWindow.EraseContent();
        window.SetActive(false);

    }
}