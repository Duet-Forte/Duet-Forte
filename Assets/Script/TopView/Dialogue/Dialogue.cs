using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public struct Dialogue
{
    private int id;
    private string[] lines;
    private string[] speakers;
    private string currentSpeaker;

    private const int CONTEXT_PART_INDEX = 0;
    private const int NAME_PART_INDEX = 1;
    public Dialogue(int id, string[] lines)
    {
        this.id = id;
        this.lines = lines;
        speakers = new string[lines.Length];
        currentSpeaker = null;

        for(int i = 0; i < lines.Length; ++i)
        {
            string[] speakerString = lines[i].Split(";");
            if (speakerString.Length > NAME_PART_INDEX)
                speakers[i] = speakerString[NAME_PART_INDEX];
            this.lines[i] = speakerString[CONTEXT_PART_INDEX];
        }
    }
    public string[] Lines { get => lines; }
    public string[] Speakers { get => speakers; }
    public string Speaker { get => currentSpeaker; set => currentSpeaker = value; }

    public Sprite Sprite { get => DataBase.Instance.Dialogue.GetSprite(currentSpeaker);}
}
