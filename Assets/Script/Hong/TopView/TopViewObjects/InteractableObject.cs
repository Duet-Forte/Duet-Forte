using System;
using UnityEngine;

public class InteractableObject : TopViewObject, IInteractable
{
    //��ȭ�� �� �� �� ������ �������� ��ȭ�� ���. ����Ʈ ����, ���丮 ���� ��� �ش� ���� ����.
    private int currentContextID;
    public override void InitSettings(string name, int id = 0)
    {
        base.InitSettings(name, id);
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
    public async void InteractPlayer(PlayerController player)
    {
        await DialogueManager.Instance.Talk("Cutscene", currentContextID);
        player.IsStopped = false;
    }
}
