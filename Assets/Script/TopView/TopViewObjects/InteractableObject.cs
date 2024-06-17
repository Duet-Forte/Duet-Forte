using System;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : TopViewEntity, IInteractable
{
    //��ȭ�� �� �� �� ������ �������� ��ȭ�� ���. ����Ʈ ����, ���丮 ���� ��� �ش� ���� ����.
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
