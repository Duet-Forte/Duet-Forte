using UnityEngine;
using Util;

public struct DialogueEvent
{
    public int assignChecker;
    public Util.CustomEnum.EventType type;
    public GameObject eventTarget;
    public int trigger;

    public DialogueEvent(string eventData)
    {
        string[] parsedData = eventData.Split('/');

        switch (parsedData[0])
        {
            case "Emotion":
                type = Util.CustomEnum.EventType.Emotion;
                break;
            default:
                type = Util.CustomEnum.EventType.None;
                break;
        }

        eventTarget = null;
        switch (parsedData[1])
        {
            case "QuestionMark":
                trigger = Const.questionHash;
                break;
            case "Surprise":
                trigger = Const.surpriseHash;
                break;
            case "Dumbfounded":
                trigger = Const.dumbHash;
                break;
            case "Angry":
                trigger = Const.angryHash;
                break;
            case "Dust":
                trigger = Const.dustHash;
                break;
            default:
                trigger = -1;
                break;
        }
        assignChecker = -1;
    }
}
