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

    public float cutoff = 0;
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _image = gameObject.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        _image.materialForRendering.SetFloat(_SizeId, cutoff);
    }
}
