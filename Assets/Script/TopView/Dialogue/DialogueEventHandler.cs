using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class DialogueEventHandler
{
    public DialogueEvent dialogueEvent;
    public bool isDone;
    public bool isSkippable { get { if (dialogueEvent is EmotionEvent) return true; else return false;  } }

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

        Type type = typeMappings.TryGetValue(parsedData[0], out var mappedType) ? mappedType : null;
        if(type != null)
        {
            isDone = false;
            dialogueEvent = Activator.CreateInstance(type) as DialogueEvent;
            dialogueEvent.InitSettings(parsedData[1]);
        }
    }

    /// <summary>
    /// 다음 대사창이 자동으로 나오는 여부를 결과값으로 반환.
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="interactorName"></param>
    /// <returns></returns>
    public bool PlayEvent(Dialogue dialogue, string interactorName)
    {
        dialogueEvent.PlayEvent(dialogue, interactorName);
        isDone = true;
        if (dialogueEvent is SkillEvent)
            return false;
        else
            return true;
    }
}


public abstract class DialogueEvent
{
    protected string eventTrigger;
    public virtual void InitSettings(string eventTrigger) => this.eventTrigger = eventTrigger;
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
            controller = GameManager.FieldManager.Field.GetCutsceneObject(dialogue.Speaker).GetComponent<TopViewEventController>();
        else if (dialogue.Speaker == "Zio")
            controller = GameManager.FieldManager.Player.GetComponent<TopViewEventController>();
        else
        {
            InteractableObject eventTarget = GameManager.FieldManager.Field.GetEntity(dialogue.Speaker) as InteractableObject;
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
        if (!DataBase.Quest.Quests.ContainsKey(QuestManager.Instance.GetQuest(int.Parse(eventTrigger))))
            QuestManager.Instance.SetQuest(int.Parse(eventTrigger));
    }
}

public class SkillEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        DataBase.Skill.ActivateSkill(int.Parse(eventTrigger));
    }
}

public class FlowEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        DataBase.Dialogue.PlusID(interactorName, int.Parse(eventTrigger));
    }
}

public class BattleEvent : DialogueEvent
{
    public override void PlayEvent(Dialogue dialogue, string interactorName)
    {
        GameManager.CutsceneManager.PauseDirector();
        GameManager.Instance.SetBattleScene(eventTrigger);
    }
}