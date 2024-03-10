using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AkSoundEngine.PostEvent("Player_Step", gameObject);
            AkSoundEngine.SetSwitch("State", "Wood", gameObject);
        }
    }
}
