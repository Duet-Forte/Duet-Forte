using UnityEngine;

public class Setting : Menu
{
    public override void OnPressed()
    {
        AkSoundEngine.PostEvent("MainMenu_Click_SFX", gameObject);
        Debug.Log("¼³Á¤");
    }
}
