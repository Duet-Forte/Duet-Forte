using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class BlockEvent : EventTrigger
{
    private PlayerController controller;
    [SerializeField] private Transform targetPoint;
    protected override void RunTask()
    {
        if (controller == null) 
        {
            controller = SceneManager.Instance.FieldManager.Player.GetComponent<PlayerController>();
        }
        controller.Stop();
        SceneManager.Instance.CutsceneManager.FadeIn(0.5f, () => OnFinishFade());
    }

    private void OnFinishFade()
    {
        SceneManager.Instance.FieldManager.Player.transform.position = targetPoint.position;
        SceneManager.Instance.CutsceneManager.FadeOut(0.5f);
        Debug.Log("여기는 지금 못가는 것 같아");
        controller.IsStopped = false;
    }
}
