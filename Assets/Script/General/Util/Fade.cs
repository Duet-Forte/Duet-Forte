using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Fade : StateMachineBehaviour
{
    [SerializeField] private bool isFadeIn; 
    private Image image;
    private WipeAnimation wipeAnimation;
    private int sizeID;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (image == null)
        {
            image = animator.GetComponent<Image>();
            sizeID = Shader.PropertyToID("_cutoff");
            wipeAnimation = animator.GetComponent<WipeAnimation>();
        }
        
        if (isFadeIn)
            AkSoundEngine.PostEvent("Transition_FadeIn", animator.gameObject);
        else
            AkSoundEngine.PostEvent("Transition_FadeOut", animator.gameObject);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        image.materialForRendering.SetFloat(sizeID, wipeAnimation.cutoff);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wipeAnimation.OnFadeFinish();
    }
}
