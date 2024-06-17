using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class DialogueEventHandler
{
    public Type type;
    public int trigger;
    public DialogueEvent dialogueEvent;

    private static Dictionary<string, Type> typeMappings = new Dictionary<string, Type>
    {
        { "Emotion", typeof(EmotionEvent)},
        { "Quest", typeof(QuestEvent)},
        { "Skill", typeof(SkillEvent)},
        { "Plus", typeof(FlowEvent)}
        // 필요한 경우 추가 타입 매핑
    };

    private static Dictionary<string, int> triggerMappings = new Dictionary<string, int>
    {
        { "QuestionMark", Const.questionHash },
        { "Surprise", Const.surpriseHash },
        { "Dumbfounded", Const.dumbHash },
        { "Angry", Const.angryHash },
        { "Dust", Const.dustHash },
        { "Drink", Const.drinkHash },
    };

    public DialogueEventHandler(string eventData)
    {
        string[] parsedData = eventData.Split('/');

        type = typeMappings.TryGetValue(parsedData[0], out var mappedType) ? mappedType : null;
        trigger = triggerMappings.TryGetValue(parsedData[1], out var mappedTrigger) ? mappedTrigger : int.Parse(parsedData[1]);
        if(type != null)
        {
            dialogueEvent = Activator.CreateInstance(type) as DialogueEvent;
            dialogueEvent.InitSettings(trigger);
        }
    }

    public void PlayEvent(Dialogue dialogue, string interactorName)
    {
        dialogueEvent.PlayEvent(dialogue, interactorName);
    }
}


public abstract class DialogueEvent
{
    protected int eventTrigger;
    public void InitSettings(int eventTrigger) => this.eventTrigger = eventTrigger;
    public abstract void PlayEvent(Dialogue dialogue, string interactorName);
}

public class EmotionEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        TopViewEventController controller;
        if (interactorName == "Cutscene")
            controller = SceneManager.Instance.FieldManager.Field.GetCutsceneObject(dialogue.Speaker).GetComponent<TopViewEventController>();
        else if (dialogue.Speaker == "Zio")
            controller = SceneManager.Instance.FieldManager.Player.GetComponent<TopViewEventController>();
        else
        {
            InteractableObject eventTarget = SceneManager.Instance.FieldManager.Field.GetEntity(dialogue.Speaker) as InteractableObject;
            controller = eventTarget.Controller;
        }
        controller.InitSettings();
        controller.PlayEvent(eventTrigger);
    }
}

public class QuestEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        if (!DataBase.Instance.Player.Quests.Contains(QuestManager.Instance.GetQuest(eventTrigger)))
            QuestManager.Instance.SetQuest(eventTrigger);
    }
}

public class SkillEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        DataBase.Instance.Skill.ActivateSkill(eventTrigger);
    }
}

public class FlowEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        DataBase.Instance.Dialogue.PlusID(interactorName, eventTrigger);
    }
}

