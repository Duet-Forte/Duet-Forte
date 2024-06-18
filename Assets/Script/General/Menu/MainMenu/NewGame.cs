using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NewGame : Menu
{
    [SerializeField] private Image fadeOut;

    public override void OnSelected()
    {
        AkSoundEngine.PostEvent("MainMenu_Hover_SFX", gameObject);
    }
    
    public override void OnPressed()
    {
        if(!(selector as MainMenu).isGameStarted)
        {
            (selector as MainMenu).isGameStarted = true;
            AkSoundEngine.PostEvent("MainMenu_Click_GameStart_SFX", gameObject);
            AkSoundEngine.PostEvent("MainMenu_BGM_BGM_Stop", transform.parent.gameObject);
            fadeOut.DOFade(1, 2f).onComplete += () => UnityEngine.SceneManagement.SceneManager.LoadScene("Top View");
        }
    }
}
