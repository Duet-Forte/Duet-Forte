using Util;
using UnityEngine;
using UnityEditor.Rendering;

public class QTE : MonoBehaviour
{
    private int startTriggerHash = Animator.StringToHash("isStart");
    private int animationSpeedHash = Animator.StringToHash("Speed");
    private int endTrigger = Animator.StringToHash("isEnd");
    private bool isQte = false;

    private Animator animator;
    private float animationPlaySpeed;
    private CustomEnum.QTEJudge judge;
    public CustomEnum.QTEJudge Judge { set { judge = value; } }
    public Animator Animator { get { return animator; } }
    public float Speed { get { return animationPlaySpeed; } }
    public void InitSettings(float secondsPerBeat)
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        animationPlaySpeed = secondsPerBeat;
    }
    public void StartQTE(Vector2 targetPosition)
    {
        isQte = true;
        gameObject.SetActive(true);
        transform.position = targetPosition;
        animator.SetTrigger(startTriggerHash);
        animator.SetFloat(animationSpeedHash, animationPlaySpeed);
        JudgeMiss();
    }
    public void EndQTE()
    {
        isQte = false;
        switch (judge)
        {
            case CustomEnum.QTEJudge.Perfect:
                Debug.Log("Perfect!");
                break;
            case CustomEnum.QTEJudge.Good:
                Debug.Log("Good");
                break;
            case CustomEnum.QTEJudge.Miss:
                Debug.Log("Miss");
                break;
            default:
                break;
        }
        isQte = false;
        gameObject.SetActive(false);
        return;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.F)||Input.GetKeyDown(KeyCode.J))&&isQte)
        {
            animator.SetTrigger(endTrigger);
            Debug.Log("지금 누름");
        }
    }
    public void JudgePerfect() => judge = CustomEnum.QTEJudge.Perfect;
    public void JudgeGood() => judge = CustomEnum.QTEJudge.Good;
    public void JudgeMiss() => judge = CustomEnum.QTEJudge.Miss;
    public void OnAnimationEnd() => animator.SetTrigger(endTrigger);
}
