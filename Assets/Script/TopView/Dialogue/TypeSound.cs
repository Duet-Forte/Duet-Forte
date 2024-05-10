using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSound : MonoBehaviour
{
    public void MakeTypeSound(char dummy)
    {
        AkSoundEngine.PostEvent("Ui_Typing_SFX", gameObject);   
    }
}
