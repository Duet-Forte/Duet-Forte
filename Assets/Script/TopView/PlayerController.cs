using Cysharp.Threading.Tasks;
using SoundSet;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
//�׽�Ʈ Ŭ����

public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private float velocity = 3f;
    [SerializeField] private float rayIntensity = 1.0f;
    private float h;
    private float v;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 lastDirection;

    private int xDirectionAnimationHash;
    private int yDirectionAnimationHash;
    private int xIdleAnimationHash;
    private int yIdleAnimationHash;
    private int moveAnimationHash;

    private bool canInteract;
    private IInteractable interactable;
    private bool isStopped;
    public bool IsStopped { get => isStopped; set => isStopped = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SetAnimatorHash();
        lastDirection = new Vector2(0, -1);
        canInteract = false;
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Stop();
            canInteract = false;
            interactable?.InteractPlayer(this);
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(h, v);

        if (!isStopped)
        {
            rb.velocity = direction.normalized * velocity;
            SetAnimatorFloat(direction);
            SetRay(lastDirection);
        }
    }

    // �ִϸ����� �Ķ���� �ؽ�
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
        int x = (int)direction.x;
        int y = (int)direction.y;

        anim.SetFloat(xDirectionAnimationHash, x);
        anim.SetFloat(yDirectionAnimationHash, y);

        // ���� �÷��̾� �Է��� ������ �ʾҴٸ�,
        // �ִϸ������� Idle ������ ������ ���������� �ٶ� ������ ������.
        // ���� move�� ���� false�� �����ؼ� Idle ���¸� ������.
        if (x.Equals(0) && y.Equals(0))
        {
            anim.SetFloat(xIdleAnimationHash, lastDirection.x);
            anim.SetFloat(yIdleAnimationHash, lastDirection.y);
            anim.SetBool(moveAnimationHash, false);
        }
        // �ݴ�� �������ٸ� Move ������ ������ �����ϰ�
        // move�� ���� true�� �����Ͽ� Move ���¸� ������.
        else
        {
            anim.SetBool(moveAnimationHash, true);
            lastDirection = new Vector2(x, y);
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
        h = 0;
        v = 0;
        anim.SetBool(moveAnimationHash, false);
    }
  
}
