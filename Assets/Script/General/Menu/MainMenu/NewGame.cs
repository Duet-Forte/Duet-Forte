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
        AkSoundEngine.PostEvent("MainMenu_Click_GameStart_SFX", gameObject);
        Debug.Log("»õ °ÔÀÓ");
        fadeOut.DOFade(1, 2f).onComplete += () => UnityEngine.SceneManagement.SceneManager.LoadScene("Top View");
    }
}
