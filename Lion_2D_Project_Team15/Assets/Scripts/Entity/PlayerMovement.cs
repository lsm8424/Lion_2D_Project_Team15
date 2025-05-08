using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed;      // 이동 속도
    public float jumpForce;      // 점프 힘
    public bool isGrounded;      // 바닥 접지 상태
    public bool facingRight = true; // 캐릭터 시작 시 바라보는 방향

    [Header("상태")]
    public bool isStunned = false;   // 경직 상태

    private Rigidbody2D rb;
    private Animator anim;

    [Header("무기 참조")]
    public Sword sword; // Sword 참조

    // 공격 상태 관리 플래그
    public bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleMove()
    {
        if (isStunned) 
            return;

        // 공격 중이고 점프 상태가 아닐 경우 이동 차단
        if (isAttacking && !anim.GetBool("Jump"))
            return;

        // 사다리 위에 있을 때는 좌/우 이동 차단
        if (Player.Instance.interaction.IsOnLadder())
            return;

        float h = Input.GetAxisRaw("Horizontal");
        Vector3 moveDir = new Vector3(h, 0, 0).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        FlipByDirection(h);

        bool isRunning = h != 0;
        anim.SetBool("Run", isRunning);
        if (sword != null) sword.SetRun(isRunning);
    }

    public void HandleJump()
    {
        if (isStunned) return;

        // 사다리 위에서는 점프 금지 (PlayerInteraction이 스페이스로 탈출 처리)
        if (Player.Instance.interaction.IsOnLadder())
            return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            anim.SetBool("Jump", true);
            if (sword != null) sword.SetJump(true);
        }
    }

    public void FlipByDirection(float h)
    {
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (sword != null) sword.Flip(facingRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        anim.SetBool("Jump", false);
        if (sword != null) sword.SetJump(false);
    }
}
