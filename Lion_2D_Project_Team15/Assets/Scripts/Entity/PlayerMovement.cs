using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // 이동속도
    public float jumpForce; // 점프 힘
    public bool isGrounded; // 땅에 닿아 있는지 여부
    public bool facingRight = true; // 오른쪽 바라보고 시작

    private Rigidbody2D rb;
    private Animator anim;

    [Header("참조")]
    public Sword sword; // Sword 참조


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 moveDir = new Vector3(h, 0, 0).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

        bool isRunning = h != 0;
        anim.SetBool("Run", isRunning);
        if (sword != null) sword.SetRun(isRunning); // Sword에 전달

        FlipByDirection(h);
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
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        if (sword != null) sword.Flip(facingRight); // Sword에도 Flip 전달
    }


    public void HandleJump()
    {
        // 스페이스 키 입력 && 땅에 있는 상태일 경우만 점프 가능
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); // 위쪽으로 힘을 가함
            isGrounded = false; // 공중 상태로 전환

            anim.SetBool("Jump", true);
            if (sword != null) sword.SetJump(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 대상이 Ground 태그를 가지고 있다면 착지 처리
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true; // 다시 점프 가능 상태로 설정

        anim.SetBool("Jump", false);
        if (sword != null) sword.SetJump(false);
    }
}
