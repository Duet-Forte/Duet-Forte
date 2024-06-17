using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class DialogueEventHandler
{
    public Type type;
    public DialogueEvent dialogueEvent;

    private static Dictionary<string, Type> typeMappings = new Dictionary<string, Type>
    {
        { "Emotion", typeof(EmotionEvent)},
        { "Quest", typeof(QuestEvent)},
        { "Skill", typeof(SkillEvent)},
        { "Plus", typeof(FlowEvent)},
        { "Battle", typeof(BattleEvent)}
        // 필요한 경우 추가 타입 매핑
    };

    public DialogueEventHandler(string eventData)
    {
        string[] parsedData = eventData.Split('/');

        type = typeMappings.TryGetValue(parsedData[0], out var mappedType) ? mappedType : null;
        if(type != null)
        {
            dialogueEvent = Activator.CreateInstance(type) as DialogueEvent;
            dialogueEvent.InitSettings(parsedData[1]);
        }
    }

    public void PlayEvent(Dialogue dialogue, string interactorName)
    {
        dialogueEvent.PlayEvent(dialogue, interactorName);
    }
}


public abstract class DialogueEvent
{
    protected string eventTrigger;
    public void InitSettings(string eventTrigger) => this.eventTrigger = eventTrigger;
    public abstract void PlayEvent(Dialogue dialogue, string interactorName);
}

public class EmotionEvent : DialogueEvent
{
    private static Dictionary<string, int> triggerMappings = new Dictionary<string, int>
    {
        { "QuestionMark", Const.questionHash },
        { "Surprise", Const.surpriseHash },
        { "Dumbfounded", Const.dumbHash },
        { "Angry", Const.angryHash },
        { "Dust", Const.dustHash },
        { "Drink", Const.drinkHash },
    };
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        if (!triggerMappings.TryGetValue(eventTrigger, out int emotionTrigger))
        {
            Debug.Log($"해당하는 애니메이션이 없습니다! : {eventTrigger}");
            return;
        }
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
        controller.PlayEvent(emotionTrigger);
    }
}

public class QuestEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        if (!DataBase.Instance.Player.Quests.Contains(QuestManager.Instance.GetQuest(int.Parse(eventTrigger))))
            QuestManager.Instance.SetQuest(int.Parse(eventTrigger));
    }
}

public class SkillEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        DataBase.Instance.Skill.ActivateSkill(int.Parse(eventTrigger));
    }
}

public class FlowEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        DataBase.Instance.Dialogue.PlusID(interactorName, int.Parse(eventTrigger));
    }
}

public class BattleEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        SceneManager.Instance.CutsceneManager.PauseDirector();
        SceneManager.Instance.SetBattleScene(eventTrigger);
    }
}