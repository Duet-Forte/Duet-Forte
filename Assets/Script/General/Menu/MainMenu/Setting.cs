using UnityEngine;

public class Setting : Menu
{
    public override void OnSelected()
    {
        AkSoundEngine.PostEvent("MainMenu_Hover_SFX", gameObject);
    }
    public override void OnPressed()
    {
        AkSoundEngine.PostEvent("MainMenu_Click_SFX", gameObject);
        Debug.Log("����");
    }
}
