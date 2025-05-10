using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed;
    public float jumpForce;
    public bool isGrounded;
    public bool facingRight = true;
    public bool canJump = true;

    [Header("접지 체크")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("벽 체크 추가")]
    public Transform wallCheck;
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.3f;
    private bool isTouchingWall;

    private Rigidbody2D rb;
    private Animator anim;

    [Header("무기 참조")]
    public Sword sword;

    public bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleMove()
    {
        if (Player.Instance.IsStunned)
            return;
        if (isAttacking && !anim.GetBool("Jump"))
            return;
        if (Player.Instance.interaction.IsOnLadder())
            return;

        float h = Input.GetAxisRaw("Horizontal");
        Vector3 moveDir = new Vector3(h, 0, 0).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        FlipByDirection(h);

        bool isRunning = h != 0;
        anim.SetBool("Run", isRunning);
        if (sword != null)
            sword.SetRun(isRunning);
    }

    public void HandleJump()
    {
        if (!canJump)
            return;
        if (Player.Instance.interaction.IsOnLadder())
            return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);
            if (sword != null)
                sword.SetJump(true);
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

        if (sword != null)
            sword.Flip(facingRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        anim.SetBool("Jump", false);
        if (sword != null)
            sword.SetJump(false);
    }

    private void Update()
    {
        // GameManager의 EntityTimeScale이 0이면 애니메이션을 Idle로 변경하고 멈춤
        if (GameManager.Instance.EntityTimeScale == 0f)
        {
            // 모든 애니메이션 파라미터를 false로 설정
            anim.SetBool("Run", false);
            anim.SetBool("Jump", false);
            if (sword != null)
            {
                sword.SetRun(false);
                sword.SetJump(false);
            }
            // 애니메이션 정지
            anim.speed = 0f;
            return;
        }

        // 일반 상태에서는 애니메이션 속도 복구
        anim.speed = 1f;

        // 벽에 붙은 상태가 아니라면 접지 여부 확인
        isTouchingWall = Physics2D.Raycast(
            wallCheck.position,
            facingRight ? Vector2.right : Vector2.left,
            wallCheckDistance,
            wallLayer
        );
        isGrounded =
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)
            && !isTouchingWall;
    }
}
