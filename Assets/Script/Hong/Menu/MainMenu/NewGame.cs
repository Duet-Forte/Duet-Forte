using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NewGame : Menu
{
    [SerializeField] private Image fadeOut;

    public override void OnPressed()
    {
        AkSoundEngine.PostEvent("MainMenu_Click_SFX", gameObject);
        Debug.Log("�� ����");
        fadeOut.DOFade(1, 2f).onComplete += () => UnityEngine.SceneManagement.SceneManager.LoadScene("Top View");
    }
}
