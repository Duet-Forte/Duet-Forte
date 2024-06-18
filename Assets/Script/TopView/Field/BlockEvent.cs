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
    protected override void RunTask()
    {
        if (DataBase.Instance.Player.Quests.Contains(requiredQuest))
            return;
        if (controller == null) 
        {
            controller = BICSceneManager.Instance.FieldManager.Player.GetComponent<PlayerController>();
        }
        controller.Stop();
        BICSceneManager.Instance.CutsceneManager.FadeIn(0.5f, () => OnFinishFade().Forget());
    }

    private async UniTask OnFinishFade()
    {
        BICSceneManager.Instance.FieldManager.Player.transform.position = targetPoint.position;
        BICSceneManager.Instance.CutsceneManager.FadeOut(0.5f);
        if (forceDialogue)
            await DialogueManager.Instance.Talk(dialogueTarget);
        else
            await DialogueManager.Instance.Talk("Zio");
        controller.IsStopped = false;
    }
}
