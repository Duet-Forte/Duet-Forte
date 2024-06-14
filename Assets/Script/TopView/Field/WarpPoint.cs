using Util;
using UnityEngine;

public class WarpPoint : EventTrigger
{
    [SerializeField] private Transform destination;
    [SerializeField] private bool isChangeMusic;
    protected override void RunTask()
    {
        if(SceneManager.Instance.FieldManager.Player.layer == Const.WARPED_PLAYER_LAYER)
        {
            SceneManager.Instance.FieldManager.Player.layer = Const.PLAYER_LAYER;
            return;

        }
        SceneManager.Instance.FieldManager.Player.layer = Const.WARPED_PLAYER_LAYER;
        SceneManager.Instance.FieldManager.Player.transform.position = destination.position;
        if (isChangeMusic)
            SceneManager.Instance.MusicChanger.SetMusic(name);
        SceneManager.Instance.CameraManager.ChangeCameraCollider(name);
    }
}
