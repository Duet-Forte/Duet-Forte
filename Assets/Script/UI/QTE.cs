using Util.CustomEnum;
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
    private QTEJudge judge;
    public QTEJudge Judge { set { judge = value; } }
    public Animator Animator { get { return animator; } }
    public float Speed { get { return animationPlaySpeed; } }
    public void InitSettings(float secondsPerBeat)
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        animationPlaySpeed = secondsPerBeat;
        animationPlaySpeed = (float)Metronome.instance.SecondsPerBeat;
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
            case QTEJudge.Perfect:
                Debug.Log("Perfect!");
                break;
            case QTEJudge.Good:
                Debug.Log("Good");
                break;
            case QTEJudge.Miss:
                Debug.Log("Miss");
                break;
            default:
                break;
        }
        isQte = false;
        gameObject.SetActive(false);
        return;
    }
    public bool getQTEEnd()
    {
        return !isQte;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J)) && isQte)
        {
            animator.SetTrigger(endTrigger);
            Debug.Log("지금 누름");
            isQte = false;
        }
    }
    public void JudgePerfect() => judge = QTEJudge.Perfect;
    public void JudgeGood() => judge = QTEJudge.Good;
    public void JudgeMiss() => judge = QTEJudge.Miss;
    public void OnAnimationEnd() => animator.SetTrigger(endTrigger);
}
