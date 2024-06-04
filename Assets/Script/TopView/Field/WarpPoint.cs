using Util;
using UnityEngine;

public class WarpPoint : EventTrigger
{
    [SerializeField] private Transform destination;
    [SerializeField] private bool isChangeMusic;
    protected override void RunTask()
    {
        SceneManager.Instance.FieldManager.Player.layer = Const.WARPED_PLAYER_LAYER;
        SceneManager.Instance.FieldManager.Player.transform.position = destination.position;
        if (isChangeMusic)
            SceneManager.Instance.MusicChanger.SetMusic(name);
        SceneManager.Instance.CameraManager.ChangeCameraCollider(name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == Const.WARPED_PLAYER_LAYER)
            collision.gameObject.layer = Const.PLAYER_LAYER;
    }
}
