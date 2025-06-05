using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepHandler : MonoBehaviour
{
    [Header("발걸음 사운드")]
    [Tooltip("모래 위에서 재생할 효과음")]
    public AudioClip sandStepClip;

    [Header("지면 감지")]
    [Tooltip("발밑 지면 검사용 위치 (발바닥 근처)")]
    public Transform groundCheck;
    [Tooltip("지면 감지 반경")]
    public float checkRadius = 0.3f;
    [Tooltip("지면 레이어 (예: 'Ground'만 체크)")]
    public LayerMask groundLayer;

    [Header("걸음 소리 타이밍")]
    [Tooltip("한 걸음당 최소 재생 간격 (초)")]
    public float stepInterval = 0.5f;

    private float stepTimer = 0f;
    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // 1) groundCheck 위치에서 반경만큼 Physics2D.OverlapCircle. Ground 레이어만 검사
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        bool isGrounded = (hit != null);

        // 2) “이동 중” 여부는 입력값을 기준으로 판단 (velocity 대신 Input 사용 → 유연)
        bool isMovingHorizontally = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f;

        if (isGrounded && isMovingHorizontally)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;
                // 감지된 지면이 “Sand” 태그이면 sandStepClip 재생
                if (hit.CompareTag("Sand") && sandStepClip != null)
                {
                    audioSrc.PlayOneShot(sandStepClip);
                }
            }
        }
        else
        {
            // 이동을 멈추거나 공중에 떠 있으면 곧바로 다시 재생하도록 타이머 리셋
            stepTimer = stepInterval;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Scene 뷰에 감지 반경 시각화 (디버그 용도)
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
