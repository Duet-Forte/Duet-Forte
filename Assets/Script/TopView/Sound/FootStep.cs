using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FootStep : MonoBehaviour
{
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
            {
                ChangeGround(hit.collider.gameObject.GetComponent<Ground>().groundType);
                return;
            }
        }
    }
    public void ChangeGround(string name)
    {
        Debug.Log(name);
        AkSoundEngine.SetSwitch("State", name, gameObject);
    }
}
