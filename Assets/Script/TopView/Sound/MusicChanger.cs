using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger
{
    private GameObject gameObject;
    private string beforeSong;
    public MusicChanger(GameObject gameManagerObject)
    {
        gameObject = gameManagerObject;
    }
    public void SetMusic(string name)
    {
        if (beforeSong == name)
            return;
        WaitMusic(name).Forget();
    }
    public async UniTask WaitMusic(string name)
    {
        await UniTask.WaitWhile(() => !AkSoundEngine.IsInitialized());
        AkSoundEngine.PostEvent("NonCombat_BGM_Stop", gameObject);
        AkSoundEngine.SetSwitch("NonCombatBGM", name, gameObject);
        AkSoundEngine.PostEvent("NonCombat_BGM", gameObject);
        beforeSong = name;
    }
    public void StopMusic() => AkSoundEngine.PostEvent("NonCombat_BGM_Stop", gameObject);
    public void ReplayMusic()
    {
        AkSoundEngine.SetSwitch("NonCombatBGM", beforeSong, gameObject);
        AkSoundEngine.PostEvent("NonCombat_BGM", gameObject);
    }
}
