using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WipeAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _animator;
    private Image _image;
    private readonly int _SizeId = Shader.PropertyToID("_cutoff");
    [SerializeField] GameObject parentObject;
    Tween fadeOut;
    Tween fadeIn;
    public float maxCutOff;
    public float minCutOff; 
    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _image = gameObject.GetComponent<Image>();
        fadeOut = _image.materialForRendering.DOFloat(maxCutOff, _SizeId, 2f);
        fadeIn = _image.materialForRendering.DOFloat(minCutOff, _SizeId, 2f);
    }

    public void FadeOut(Action onFadeOut = null)
    {
        parentObject.GetComponent<Canvas>().worldCamera = Camera.main;
        _image.materialForRendering.SetFloat(_SizeId, minCutOff);
        Sequence temp = DOTween.Sequence();

        temp.Append(fadeOut);

        temp.AppendCallback(() => { onFadeOut?.Invoke();});

        temp.Play();
    }

    public void FadeIn(Action onFadeIn = null)
    {
        parentObject.GetComponent<Canvas>().worldCamera = Camera.main;
        _image.materialForRendering.SetFloat(_SizeId, maxCutOff);
        Sequence temp = DOTween.Sequence();

        temp.Append(fadeIn);

        temp.AppendCallback(() => { onFadeIn?.Invoke(); });

        temp.Play();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
