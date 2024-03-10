using Cysharp.Threading.Tasks;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager
{
    private static DialogueManager instance;

    private GameObject window;
    private Image characterSprite;
    private TextMeshProUGUI talkerName;
    private TextMeshProUGUI contentText;
    private TypewriterByCharacter typewriter;
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
                talkerName.text = dialogue.Speaker.Split("_")[0];
                if (dialogue.Speaker.Split("_")[0] == "Empty")
                    talkerName.text = string.Empty;
                if (dialogue.Sprite != null)
                    characterSprite.sprite = dialogue.Sprite;
                else
                    characterSprite.enabled = false;
            }
            if (dialogue.Speaker == "Empty")
                talkerName.text = string.Empty;
            await UniTask.Delay(500);
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }
        window.SetActive(false);
    }
}
