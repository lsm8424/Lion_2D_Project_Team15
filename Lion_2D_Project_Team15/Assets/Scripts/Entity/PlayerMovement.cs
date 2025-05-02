using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed;      // 이동 속도
    public float jumpForce;      // 점프 힘
    public bool isGrounded;      // 바닥 접지 상태
    public bool facingRight = true; // 캐릭터 시작 시 바라보는 방향

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleMove()
    {
        // 사다리 위에 있을 때는 좌/우 이동 차단
        if (Player.Instance.interaction.IsOnLadder())
            return;

        float h = Input.GetAxisRaw("Horizontal");
        Vector3 moveDir = new Vector3(h, 0, 0).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        // 방향 전환 처리
        if (h != 0)
        {
            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();
        }
    }

    public void HandleJump()
    {
        // 사다리 위에서는 점프 금지 (PlayerInteraction이 스페이스로 탈출 처리)
        if (Player.Instance.interaction.IsOnLadder())
            return;

        // 스페이스 키 입력 && 바닥에 닿아 있을 때만 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false; // 공중 상태로 전환
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
