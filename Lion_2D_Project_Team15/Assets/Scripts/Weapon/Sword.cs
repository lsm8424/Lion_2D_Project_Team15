using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Sword 위치 동기화")]
    public Transform hand; // 손 위치를 참조
    public Vector3 offset; // 상대 위치 조정 (필요 시 추가)

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (hand == null)
        {
            Debug.LogError("Hand Transform이 설정되지 않았습니다!");
        }
    }

    private void Update()
    {
        if (hand != null)
        {
            // Sword를 손 위치로 동기화
            transform.position = hand.position + offset;
        }
    }

    public void SetRun(bool isRunning)
    {
        anim.SetBool("Run", isRunning);
    }

    public void SetJump(bool isJumping)
    {
        anim.SetBool("Jump", isJumping);
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void Flip(bool facingRight)
    {
        if (spriteRenderer != null)
        {
            // Sword의 SpriteRenderer를 뒤집음
            spriteRenderer.flipX = !facingRight;

            // Offset도 방향에 따라 반전
            offset.x = facingRight ? Mathf.Abs(offset.x) : -Mathf.Abs(offset.x);
        }
    }
}
