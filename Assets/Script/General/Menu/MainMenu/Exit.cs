using UnityEngine;

public class Exit : Menu
{
    public override void OnSelected()
    {
        AkSoundEngine.PostEvent("MainMenu_Hover_SFX", gameObject);
    }
    public override void OnPressed()
    {
        if(!(selector as MainMenu).isGameStarted)
        {
            AkSoundEngine.PostEvent("MainMenu_Click_SFX", gameObject);
            Debug.Log("게임 종료");
        }
    }
}
