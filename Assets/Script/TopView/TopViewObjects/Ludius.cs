using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ludius : TopViewEventController
{
    public void PlayDrinkSound()
    {
        AkSoundEngine.PostEvent("NPC_Ludius_Drink_SFX", gameObject);
    }
}
