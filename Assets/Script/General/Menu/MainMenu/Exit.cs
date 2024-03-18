using UnityEngine;

public class Exit : Menu
{
    public override void OnPressed()
    {
        AkSoundEngine.PostEvent("MainMenu_Click_SFX", gameObject);
        Debug.Log("게임 종료");
    }
}
