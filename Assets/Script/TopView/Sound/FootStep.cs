using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    StringBuilder sb = new StringBuilder();

    public void PlaySound(string name)
    {
        CheckGround();
        AkSoundEngine.PostEvent(name, gameObject);
    }

    public void CheckGround()
    {
        Vector2 origin = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector3.forward, Mathf.Infinity);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                ChangeGround(hit.collider.gameObject.GetComponent<Ground>().groundType);
        }
    }
    public void ChangeGround(string name)
    {
        AkSoundEngine.SetSwitch("State", name, gameObject);
    }
}
