using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger
{
    private GameObject gameObject;
    public MusicChanger(GameObject gameManagerObject)
    {
        gameObject = gameManagerObject;
    }
    public void SetMusic(string name)
    {
        AkSoundEngine.PostEvent("NonCombat_BGM_Stop", gameObject);
        AkSoundEngine.SetSwitch("NonCombatBGM", name, gameObject);
        AkSoundEngine.PostEvent("NonCombat_BGM", gameObject);
    }
}
