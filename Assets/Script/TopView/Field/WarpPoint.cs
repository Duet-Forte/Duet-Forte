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
            controller = BICSceneManager.Instance.FieldManager.Player.GetComponent<PlayerController>();
        controller.Stop();
        if (soundName != null && soundName != string.Empty)
            AkSoundEngine.PostEvent(soundName, gameObject);
        BICSceneManager.Instance.CutsceneManager.FadeIn(0.5f, () => OnFinishFade());
    }

    private void OnFinishFade()
    {
        BICSceneManager.Instance.FieldManager.Player.transform.position = destination.position;
        BICSceneManager.Instance.CutsceneManager.FadeOut(0.5f);
        BICSceneManager.Instance.FieldManager.CheckPoint();
        controller.IsStopped = false;
    }
}
