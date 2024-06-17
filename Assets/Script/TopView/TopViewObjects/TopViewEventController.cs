using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewEventController : MonoBehaviour
{
    private Animator animator;
    public void InitSettings()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayEvent(int eventHash)
    {
        animator.Play(eventHash);
    }
}
