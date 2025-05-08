using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    private bool isKnockBack = false; // 넉백 상태
    private float knockbackTimer = 0f; // 넉백 지속 시간

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 넉백 지속 시간을 줄여주고, 끝나면 이동 잠금 해제
        if (isKnockBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockBack = false;
            return; // 이동 입력 무시
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(xInput, yInput).normalized; // 방향 벡터 정규화

        rb.linearVelocity = dir * speed;

    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        rb.linearVelocity = Vector2.zero; // 기존 속도 초기화
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        isKnockBack = true;
        knockbackTimer = duration;
    }


}
