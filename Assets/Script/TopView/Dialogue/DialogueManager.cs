using Cysharp.Threading.Tasks;
using Febucci.UI;
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
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DialogueManager();
            return instance;
        }
    }
    public async UniTask Talk(string speakerName, int id)
    {
        if(window == null)
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

        window.SetActive(true);
        Dialogue dialogue = DataBase.Instance.Dialogue.GetDialogue(speakerName, id);
        await WaitKeyInput(dialogue);
    }

    private async UniTask WaitKeyInput(Dialogue dialogue)
    {
        for(int i = 0; i < dialogue.Lines.Length; i++) 
        {
            characterSprite.enabled = true;
            typewriter.ShowText(dialogue.Lines[i]);
            dialogue.Speaker = dialogue.Speakers[i];
            if(dialogue.Speaker != null)
            {
                talkerName.text = dialogue.Speaker.Split('/')[0];
                if (talkerName.text == "Empty")
                {
                    talkerName.text = string.Empty;
                    currentSpeaker = Speaker.Empty;
                }
                else if (talkerName.text == "Zio")
                    currentSpeaker = Speaker.player;
                else
                    currentSpeaker = Speaker.NPC;

                if (dialogue.Sprite != null)
                    characterSprite.sprite = dialogue.Sprite;
                else
                    characterSprite.enabled = false;
            }

            dialogueWindow.SetPosition(currentSpeaker);
            await UniTask.Delay(500);
            await UniTask.WaitUntil(IsKeyTriggered);
        }
        window.SetActive(false);
    }

    private bool IsKeyTriggered()
    {
        if (SceneManager.Instance.InputController.GetAction(PlayerAction.Interact).triggered)
            return true;
        else
            return false;
    }
}
