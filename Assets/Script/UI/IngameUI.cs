using UnityEngine;
using DG.Tweening;
public class InGameUI : MonoBehaviour// 움직이는 UI는 해당 클래스를 상속받아 사용.
{
    protected GameObject beatingFrame;
    protected Vector2 originScale;
    protected Sequence beatingSequence;
    virtual protected void BeatFrameSetting() {
        beatingFrame = transform.Find("Black").gameObject;
        originScale = beatingFrame.transform.localScale;
        beatingSequence = DOTween.Sequence().SetAutoKill(false);
        beatingSequence.Append(beatingFrame.transform.DOScale(originScale*1.1f, 0.01f));
        beatingSequence.Append(beatingFrame.transform.DOScale(originScale, 0.3f));
    }
    protected void SubscribeBeatingUISequence() {
        BeatFrameSetting();
        beatingSequence.Restart();
        Metronome.instance.OnBeating += PlayBeatUISequence;
    }
    protected void PlayBeatUISequence() {
        beatingSequence.Restart();
    }
    
}
