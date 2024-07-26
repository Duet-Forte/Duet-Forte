using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody 컴포넌트 가져오기
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); // 수평 입력 (A, D 또는 좌우 화살표)
        float moveY = Input.GetAxis("Vertical");   // 수직 입력 (W, S 또는 상하 화살표)

        Vector2 moveDirection = new Vector2(moveX, moveY); // 이동 방향 설정
        moveDirection.Normalize(); // 방향 벡터를 정규화

        // Rigidbody의 velocity를 사용하여 이동
        rb.velocity = moveDirection * moveSpeed;

    }
}
