using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : TopViewEntity, IInteractable
{
    //대화를 할 때 이 변수를 기준으로 대화를 출력. 퀘스트 수주, 스토리 진행 등올 해당 값을 변경.
    private TopViewEventController controller;
    public TopViewEventController Controller { get => controller; }
    public override void InitSettings(string name, Vector2 spawnPoint, int id = 0)
    {
        base.InitSettings(name, spawnPoint, id);
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        controller = GetComponent<TopViewEventController>();
        if(controller == null)
            controller = transform.AddComponent<TopViewEventController>();
    }
    public async void InteractPlayer(PlayerController player)
    {
        await DialogueManager.Instance.Talk(objectName);
        player.IsStopped = false;
    }
}
