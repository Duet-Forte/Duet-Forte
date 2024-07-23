using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody ������Ʈ ��������
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); // ���� �Է� (A, D �Ǵ� �¿� ȭ��ǥ)
        float moveY = Input.GetAxis("Vertical");   // ���� �Է� (W, S �Ǵ� ���� ȭ��ǥ)

        Vector2 moveDirection = new Vector2(moveX, moveY); // �̵� ���� ����
        moveDirection.Normalize(); // ���� ���͸� ����ȭ

        // Rigidbody�� velocity�� ����Ͽ� �̵�
        rb.velocity = moveDirection * moveSpeed;

    }
}
