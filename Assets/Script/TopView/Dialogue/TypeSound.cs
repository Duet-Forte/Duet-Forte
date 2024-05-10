using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSound : MonoBehaviour
{
    public int count;
    public void MakeTypeSound(char dummy)
    {
        if (count >= 1)
        {
            count = 0;
            return;
        }
        count++;
        AkSoundEngine.PostEvent("Ui_Typing_SFX", gameObject);   
    }
}
