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
            controller = SceneManager.Instance.FieldManager.Player.GetComponent<PlayerController>();
        controller.Stop();
        if (soundName != null && soundName != string.Empty)
            AkSoundEngine.PostEvent(soundName, gameObject);
        SceneManager.Instance.CutsceneManager.FadeIn(0.5f, () => OnFinishFade());
    }

    private void OnFinishFade()
    {
        SceneManager.Instance.FieldManager.Player.transform.position = destination.position;
        SceneManager.Instance.CutsceneManager.FadeOut(0.5f);
        SceneManager.Instance.FieldManager.CheckPoint();
        controller.IsStopped = false;
    }
}
