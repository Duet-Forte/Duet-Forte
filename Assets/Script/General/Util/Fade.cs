using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Fade : StateMachineBehaviour
{
    private Image image;
    private WipeAnimation wipeAnimation;
    private float previousCutoff;
    private int sizeID;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (image == null)
        {
            image = animator.GetComponent<Image>();
            sizeID = Shader.PropertyToID("_cutoff");
            wipeAnimation = animator.GetComponent<WipeAnimation>();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        image.materialForRendering.SetFloat(sizeID, wipeAnimation.cutoff);
        if (previousCutoff == wipeAnimation.cutoff)
            animator.SetTrigger("EndFade");
        previousCutoff = wipeAnimation.cutoff;
    }
}
