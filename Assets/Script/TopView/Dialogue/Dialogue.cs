using Util.CustomEnum;
using UnityEngine;

public struct Dialogue
{
    private string[] lines;
    private string[] speakers;
    private string currentSpeaker;
    private DialogueEventHandler[] dialogueEvent;

    private const int CONTEXT_PART_INDEX = 0;
    private const int NAME_PART_INDEX = 1;
    private const int EVENT_PART_INDEX = 2;
    public Dialogue(string[] lines)
    {
        this.lines = lines;
        speakers = new string[lines.Length];
        currentSpeaker = null;
        dialogueEvent = new DialogueEventHandler[lines.Length];

        for(int i = 0; i < lines.Length; ++i)
        {
            string[] speakerString = lines[i].Split(";");
            if (speakerString.Length > EVENT_PART_INDEX)
                dialogueEvent[i] = new DialogueEventHandler(speakerString[EVENT_PART_INDEX]);
            speakers[i] = speakerString[NAME_PART_INDEX];
            this.lines[i] = speakerString[CONTEXT_PART_INDEX];
        }
    }
    public string[] Lines { get => lines; }
    public string[] Speakers { get => speakers; }
    public string Speaker { get => currentSpeaker; set => currentSpeaker = value; }
    public DialogueEventHandler[] Events { get => dialogueEvent; }
    public Sprite Sprite { get => DataBase.Instance.Dialogue.GetSprite(currentSpeaker);}
}
