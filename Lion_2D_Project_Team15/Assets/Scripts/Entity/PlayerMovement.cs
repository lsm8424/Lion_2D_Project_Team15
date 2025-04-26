using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // 이동속도
    public float jumpForce; // 점프 힘
    public bool isGrounded; // 땅에 닿아 있는지 여부

    private Animator anim;

    private Rigidbody2D rb;

    public Transform groundCheck; // 플레이어 발 밑 위치
    public float groundCheckDistance = 0.2f; // 땅 감지 거리
    public LayerMask groundLayer; // Ground 레이어 설정
    private RaycastHit2D groundHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleMove()
    {
        // WASD 또는 방향키 입력 받기
        float h = Input.GetAxisRaw("Horizontal"); // A,D 또는 ←, →
        float v = Input.GetAxisRaw("Vertical"); // W,S 또는 ↑,↓

        // 입력 방향으로의 벡터 계산 (Y축은 제외하고 평면 이동)
        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        Vector3 move = moveDir * moveSpeed * Time.deltaTime;

        // 이동 애니메이션 제어
        bool isMoving = moveDir != Vector3.zero;
        anim.SetBool("Run", isMoving);

        transform.Translate(move, Space.World);

        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = h > 0 ? 1 : -1; // 오른쪽 키: 1, 왼쪽 키: -1
            transform.localScale = scale;
        }

            /*
                if (isMoving)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                }
            */
    }

    public void HandleJump()
    {
        // Raycast 결과를 여기서 판정
        isGrounded = groundHit.collider != null;

        // 스페이스 키 입력 && 땅에 있는 상태일 경우만 점프 가능
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 위쪽으로 힘을 가함

            anim.SetBool("Jump", true);  // 점프 애니메이션 시작
        }
    }

    public void CheckGround()
    {
        groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, Color.green);

        bool wasGrounded = isGrounded;
        isGrounded = groundHit.collider != null;

        // 착지 "순간"에만 Jump 애니메이션 끄기
        if (isGrounded && !wasGrounded)
        {
            anim.SetBool("Jump", false);
        }
    }
}

/*
private void OnCollisionEnter(Collision collision)
{
    Debug.Log("충돌한 물체: " + collision.gameObject.name);

    // 충돌한 대상이 Ground 태그를 가지고 있다면 착지 처리
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true; // 다시 점프 가능 상태로 설정
    }
}*/

