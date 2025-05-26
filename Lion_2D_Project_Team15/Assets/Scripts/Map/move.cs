using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;

    //넉백중
    private bool isKnockBack = false; // 넉백 상태
    private float knockbackTimer = 0f; // 넉백 지속 시간

    // 키입력 중
    public bool isKeyInput = false; // 키 입력 상태

    // 회오리 갇힘
    public bool isStuck = false; // 회오리 갇힘 상태
    private Vector3 trapCenter; // 회오리 중심 위치

    // 애니메이터
    private Animator anim;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //보스맵에서 체력 초기화
        Player.Instance.HP = Player.Instance.maxHP;
    }

    void Update()
    {
        if(GameManager.Instance.EntityTimeScale == 0)
        {
            rb.linearVelocity = Vector2.zero; // 시간 정지 시 이동 멈춤
            return;
        }

        // 키입력 상태이거나 회오리에 갇혔으면 velocity를 0으로 설정 및 이동 무시
        if (isKeyInput || isStuck)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateAnimation(Vector2.zero); // 이동 멈춘 상태의 애니메이션
            return;
        }
   
        // 넉백 지속 시간을 줄여주고, 끝나면 이동 잠금 해제
        if (isKnockBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockBack = false;

            UpdateAnimation(Vector2.zero); // 이동 멈춘 상태의 애니메이션
            return; // 이동 입력 무시
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(xInput, yInput).normalized; // 방향 벡터 정규화

        rb.linearVelocity = dir * speed;

        UpdateAnimation(dir);

    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if(isStuck || isKeyInput) return; // 회오리 갇힘 상태이거나 키 입력 중이면 넉백 적용 안함

        rb.linearVelocity = Vector2.zero; // 기존 속도 초기화
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        isKnockBack = true;
        knockbackTimer = duration;
    }

    private void UpdateAnimation(Vector2 direction)
    {
        // 방향에 따라 애니메이션 파라미터 설정
        if (anim == null) return;

        anim.SetBool("Up", direction.y > 0); // 위로 이동
        anim.SetBool("Down", direction.y < 0); // 아래로 이동
        anim.SetBool("Right", direction.x > 0); // 오른쪽 이동
        anim.SetBool("Left", direction.x < 0); // 왼쪽 이동
    }

}
