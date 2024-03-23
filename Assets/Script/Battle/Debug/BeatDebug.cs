using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BeatDebug : MonoBehaviour
{
    [SerializeField] GameObject beat;
    Vector2 originScale;
    Sequence beatingSequence;
    // Start is called before the first frame update
    void Start()
    {
        originScale = beat.transform.localScale;
        beatingSequence = DOTween.Sequence().SetAutoKill(false);
        beatingSequence.Append(beat.transform.DOScale(new Vector3 (3,3),0.01f));
        beatingSequence.Append(beat.transform.DOScale(originScale, 0.3f));
        Metronome.instance.OnBeating += PlaySequence;
    }
    void PlaySequence() {
        beatingSequence.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
