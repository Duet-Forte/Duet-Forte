using System.Collections.Generic;
using UnityEngine;
using Util;

public class DialogueEvent
{
    public Util.CustomEnum.EventType type;
    public int trigger;

    private Dictionary<string, Util.CustomEnum.EventType> typeMappings = new Dictionary<string, Util.CustomEnum.EventType>
    {
        { "Emotion", Util.CustomEnum.EventType.Emotion },
        { "Quest", Util.CustomEnum.EventType.Quest},
        { "Skill", Util.CustomEnum.EventType.Skill}
        // 필요한 경우 추가 타입 매핑
    };

    private Dictionary<string, int> triggerMappings = new Dictionary<string, int>
    {
        { "QuestionMark", Const.questionHash },
        { "Surprise", Const.surpriseHash },
        { "Dumbfounded", Const.dumbHash },
        { "Angry", Const.angryHash },
        { "Dust", Const.dustHash },
        // 필요한 경우 추가 트리거 매핑
    };

    public DialogueEvent(string eventData)
    {
        string[] parsedData = eventData.Split('/');

        type = typeMappings.TryGetValue(parsedData[0], out var mappedType) ? mappedType : Util.CustomEnum.EventType.None;
        trigger = triggerMappings.TryGetValue(parsedData[1], out var mappedTrigger) ? mappedTrigger : int.Parse(parsedData[1]);
    }
}

