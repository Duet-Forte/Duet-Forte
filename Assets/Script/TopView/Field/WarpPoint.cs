using Util;
using UnityEngine;

public class WarpPoint : EventTrigger
{
    [SerializeField] private Transform destination;
    [SerializeField] private string soundName;
    private PlayerController controller;
    protected override void RunTask()
    {
        if(controller == null)
            controller = GameManager.FieldManager.Player.GetComponent<PlayerController>();
        controller.Stop();
        if (soundName != null && soundName != string.Empty)
            AkSoundEngine.PostEvent(soundName, gameObject);
        GameManager.CutsceneManager.FadeIn(0.5f, () => OnFinishFade());
    }

    private void OnFinishFade()
    {
        GameManager.FieldManager.Player.transform.position = destination.position;
        GameManager.CutsceneManager.FadeOut(0.5f);
        GameManager.FieldManager.CheckPoint();
        controller.IsStopped = false;
    }
}
