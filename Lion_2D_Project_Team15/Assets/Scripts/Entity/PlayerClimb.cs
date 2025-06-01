using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [Header("사다리 이동 설정")]
    public float climbSpeed = 3f;

    [Header("사다리 이동 효과음")]
    [Tooltip("사다리를 오르내릴 때 재생할 효과음")]
    public AudioClip climbClip;
    [Tooltip("효과음 재생 후 다음 재생까지의 대기 시간(초)")]
    public float climbInterval = 0.5f;

    private Rigidbody2D rb;
    private AudioSource audioSrc;
    private float climbTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // AudioSource 준비
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null)
            audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.playOnAwake = false;
    }

    private void Update()
    {
        if (Player.Instance.interaction.IsOnLadder())
        {
            float vertical = Input.GetAxisRaw("Vertical"); // W/S 입력

            // 사다리 이동 시 X축 속도 고정
            rb.linearVelocityX = 0;

            if (Player.Instance.interaction.currentLadder != null)
            {
                Ladder ladder = Player.Instance.interaction.currentLadder.GetComponent<Ladder>();
                float x = ladder.centerX;
                float y = transform.position.y;

                // 사다리 꼭대기 근처 처리
                if (transform.position.y > ladder.topExitY - 1.3f)
                {
                    y = ladder.topExitY - 1.5f;
                }

                // 올라갈 때
                if (vertical > 0)
                {
                    if (transform.position.y > ladder.topExitY - 1.3f)
                    {
                        y = ladder.topExitY;
                        transform.position = new Vector2(x, y);
                        Player.Instance.interaction.ExitLadder();
                    }
                }

                // 내려갈 때
                if (vertical < 0)
                {
                    if (transform.position.y < ladder.bottomExitY)
                    {
                        y = ladder.bottomExitY;
                        transform.position = new Vector2(x, y);
                        Player.Instance.interaction.ExitLadder();
                    }
                }

                transform.position = new Vector2(x, y);
            }

            // 수직 이동 속도 설정
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);

            // ── 사다리 이동 효과음 재생 ──
            if (vertical != 0f)
            {
                climbTimer += Time.deltaTime;
                if (climbTimer >= climbInterval)
                {
                    climbTimer = 0f;
                    if (climbClip != null)
                        audioSrc.PlayOneShot(climbClip);
                }
            }
            else
            {
                // 수직 입력이 없으면 타이머 초기화
                climbTimer = climbInterval;
            }
        }
    }
}
