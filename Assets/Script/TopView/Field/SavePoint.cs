using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public int saveID;
    public Vector2 spawnPoint;
    public string pointName;

    public void InteractPlayer(PlayerController controller)
    {
        DataBase.Instance.Save(saveID);
        Debug.Log("����Ϸ�!");
        controller.IsStopped = false;
    }
}

