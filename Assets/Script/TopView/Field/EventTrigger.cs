using Util.CustomEnum;
using Util;
using UnityEngine;

public abstract class EventTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Const.PLAYER_LAYER)
            RunTask();
    }

    protected abstract void RunTask();
}
