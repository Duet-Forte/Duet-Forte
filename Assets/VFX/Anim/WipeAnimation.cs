using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WipeAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    [SerializeField] GameObject parentObject;
    public float cutoff;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Fade(bool isFadeIn, Action beforeFade = null, Action onFade = null)
    {
        parentObject.GetComponent<Canvas>().worldCamera = Camera.main;
        Sequence temp = DOTween.Sequence();

        temp.AppendCallback(() => { beforeFade?.Invoke(); });

        if (isFadeIn)
            temp.AppendCallback(() => animator.Play("FadeIn"));
        else
            temp.AppendCallback(() => animator.Play("FadeOut"));

        temp.AppendCallback(() => { onFade?.Invoke();});

        temp.Play();
    }
}
