using UnityEngine;
using Util;

public class QTECircle : StateMachineBehaviour
{
    private QTE qTEJudgeManager;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(qTEJudgeManager == null)
        {
            qTEJudgeManager = animator.GetComponent<QTE>();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        qTEJudgeManager.EndQTE();
    }
}
