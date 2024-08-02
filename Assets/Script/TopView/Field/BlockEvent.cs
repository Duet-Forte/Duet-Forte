using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class BlockEvent : EventTrigger
{
    private PlayerController controller;
    [SerializeField] private bool forceDialogue;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Quest requiredQuest;
    [SerializeField] private string dialogueTarget;
    [SerializeField] private bool isForceClear;
    protected override void RunTask()
    {
        if(isForceClear)
        {
            if (DataBase.Quest.Quests[requiredQuest])
            {
                return;
            }
        }
        else if (DataBase.Quest.Quests.ContainsKey(requiredQuest))
            return;
        if (controller == null) 
        {
            controller = GameManager.FieldManager.Player.GetComponent<PlayerController>();
        }
        controller.Stop();
        GameManager.CutsceneManager.FadeIn(0.5f, () => OnFinishFade().Forget());
    }

    private async UniTask OnFinishFade()
    {
        GameManager.FieldManager.Player.transform.position = targetPoint.position;
        GameManager.CutsceneManager.FadeOut(0.5f);
        if (forceDialogue)
            await DialogueManager.Instance.Talk(dialogueTarget);
        else
            await DialogueManager.Instance.Talk("Zio");
        controller.IsStopped = false;
    }
}
