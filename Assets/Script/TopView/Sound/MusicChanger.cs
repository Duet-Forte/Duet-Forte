using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger
{
    private GameObject gameObject;
    private string beforeSong;
    [SerializeField] private uint currentSongID;
    public MusicChanger(GameObject gameManagerObject)
    {
        gameObject = gameManagerObject;
    }
    public void SetMusic(string name)
    {
        if (beforeSong == name)
            return;
        AkSoundEngine.PostEvent("NonCombat_BGM_Stop", gameObject);
        AkSoundEngine.SetSwitch("NonCombatBGM", name, gameObject);
        currentSongID = AkSoundEngine.PostEvent("NonCombat_BGM", gameObject);
        beforeSong = name;
    }
    public void StopMusic() => AkSoundEngine.PostEvent("NonCombat_BGM_Stop", gameObject);
    public void ReplayMusic() => AkSoundEngine.PostEvent("NonCombat_BGM", gameObject);
}
