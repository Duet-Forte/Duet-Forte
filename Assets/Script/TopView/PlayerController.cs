using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Util.CustomEnum;
//테스트 클래스

public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private float velocity = 3f;
    [SerializeField] private float rayIntensity = 1.0f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direction;
    public Vector2 lastDirection;
    
    private int xDirectionAnimationHash;
    private int yDirectionAnimationHash;
    private int xIdleAnimationHash;
    private int yIdleAnimationHash;
    private int moveAnimationHash;

    private InputAction moveAction;
    private bool canInteract;
    private IInteractable interactable;
    private bool isStopped;
    public bool IsStopped { get => isStopped; set => isStopped = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SetAnimatorHash();
        lastDirection = new Vector2(0, -1);
        canInteract = false;
        GameManager.InputController.BindPlayerInputAction(PlayerAction.Move, OnMove);
        GameManager.InputController.BindPlayerInputAction(PlayerAction.Interact, OnInteract);
        moveAction = GameManager.InputController.GetAction(PlayerAction.Move);
    }

    private void FixedUpdate()
    {
        if (!isStopped)
        {
            direction = moveAction.ReadValue<Vector2>();
            rb.velocity = direction * velocity;
            SetAnimatorFloat(direction);
            SetRay(lastDirection);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            Stop();
            interactable?.InteractPlayer(this);
        }
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        if(!isStopped)
        {
            lastDirection = context.ReadValue<Vector2>();
            lastDirection = new Vector2(SetMinusOrPlus(lastDirection.x), SetMinusOrPlus(lastDirection.y));
        }
    }
    // 애니메이터 파라미터 해싱
    private void SetAnimatorHash()
    {
        xDirectionAnimationHash = Animator.StringToHash("x");
        yDirectionAnimationHash = Animator.StringToHash("y");
        xIdleAnimationHash = Animator.StringToHash("idleX");
        yIdleAnimationHash = Animator.StringToHash("idleY");
        moveAnimationHash = Animator.StringToHash("move");
    }

    private void SetAnimatorFloat(Vector2 direction)
    {
        int x = SetMinusOrPlus(direction.x);
        int y = SetMinusOrPlus(direction.y);

        anim.SetFloat(xDirectionAnimationHash, x);
        anim.SetFloat(yDirectionAnimationHash, y);

        // 만약 플레이어 입력이 들어오지 않았다면,
        // 애니메이터의 Idle 상태의 방향을 마지막으로 바라본 곳으로 설정함.
        // 또한 move의 값을 false로 유지해서 Idle 상태를 유지함.
        if (x.Equals(0) && y.Equals(0))
        {
            anim.SetFloat(xIdleAnimationHash, lastDirection.x);
            anim.SetFloat(yIdleAnimationHash, lastDirection.y);
            anim.SetBool(moveAnimationHash, false);
        }
        // 반대로 움직였다면 Move 상태의 방향을 설정하고
        // move의 값을 true로 유지하여 Move 상태를 유지함.
        else
        {
            anim.SetBool(moveAnimationHash, true);
        }
    }

    private void SetRay(Vector2 direction)
    {
        Debug.DrawRay(rb.position, direction * rayIntensity, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, rayIntensity, LayerMask.GetMask("Interactable"));

        if (hit.collider != null)
        {
            interactable = hit.collider.GetComponent<IInteractable>();
            canInteract = true;
        }
        else
            canInteract = false;
    }

    public void Stop()
    {
        isStopped = true;
        direction = Vector2.zero;
        rb.velocity = Vector2.zero;
        SetAnimatorFloat(direction);
        canInteract = false;
    }
    private int SetMinusOrPlus(float someNumber)
    {
        int answer;
        if (someNumber > 0)
            answer = 1;
        else if (someNumber < 0)
            answer = -1;
        else
            answer = 0;
        return answer;
    }
}
