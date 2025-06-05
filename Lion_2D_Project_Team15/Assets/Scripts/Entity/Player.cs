using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    // ────────────── Singleton ──────────────
    private bool isStunned = false;
    public float stunDuration = 1f;
    public bool IsInvincible => isInvincible;
    private bool isInvincible = false;
    public float invincibleDuration = 1f;
    public bool IsStunned => isStunned;

    public static Player Instance { get; private set; }

    // 넉백
    private bool isKnockBack = false;
    private float knockbackTimer = 0f;

    // 키 입력 중
    public bool isKeyInput = false;

    // 회오리 갇힘
    public bool isStuck = false;

    [Header("무적 효과")]
    public float blinkInterval = 0.2f;
    private SpriteRenderer spriteRenderer;

    [Header("발걸음 사운드")]
    [Tooltip("모래 위 발걸음 효과음")]
    public AudioClip sandStepClip;
    [Tooltip("일반 지면(흙/바닥) 위에서 재생할 효과음")]
    public AudioClip groundStepClip;

    // ────────────── Footstep Detection ──────────────
    [Header("발걸음 감지 설정")]
    [Tooltip("발소리를 재생할 때 땅을 감지할 Transform (발 위치 근처)")]
    public Transform groundCheck;
    [Tooltip("지면 감지를 위한 반경")]
    public float checkRadius = 0.2f;
    [Tooltip("발소리를 재생할 지면 레이어 (예: “Ground”에 해당)")]
    public LayerMask groundLayer;
    [Tooltip("한 걸음당 재생 간격 (초)")]
    public float stepInterval = 0.5f;

    private float stepTimer = 0f;
    private AudioSource audioSrc;

    // ────────────── 기능별 모듈 스크립트 참조 ──────────────
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerInteraction interaction;

    private void Awake()
    {
        // 싱글톤 등록
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // AudioSource 준비
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null)
            audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.playOnAwake = false;
    }

    private void Start()
    {
        // 기능별 모듈 스크립트 가져오기
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        interaction = GetComponent<PlayerInteraction>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Bind();
    }

    private void Update()
    {
        if (GameManager.Instance.ShouldWaitForEntity())
            return;

        // 키 입력 중이거나 회오리에 갇혔으면 이동 무시
        if (isKeyInput || isStuck)
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            return;
        }

        // 넉백 처리
        if (isKnockBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockBack = false;
            return;
        }

        // 이동 및 점프
        if (movement != null)
        {
            movement.HandleMove();
            movement.HandleJump();
        }

        // 공격 및 상호작용
        combat.HandleAttack();
        combat.HandleSkill();
        interaction.HandleInteraction();

        // 발걸음 소리 재생
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        // 1) “Ground” 레이어에 속한 Collider를 찾아서 땅에 있는지 체크
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        bool isGrounded = (hit != null);

        // 디버그: hit 결과 출력 (필요 시 주석 처리 가능)
        //if (isGrounded)
        //{
        //    Debug.Log($"[Footstep] Hit Ground Collider: {hit.name}, Tag: {hit.tag}");
        //}
        //else
        //{
        //    Debug.Log($"[Footstep] No ground detected at {groundCheck.position}");
        //}

        // 2) 실제 수평 이동 속도를 통해 이동 중인지 확인
        float horizVel = Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x);
        bool isMovingHorizontally = horizVel > 0.1f;

        if (isGrounded && isMovingHorizontally)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;

                // 모래 지형일 때
                if (hit.CompareTag("Sand"))
                {
                    if (sandStepClip != null)
                    {
                        Debug.Log("[Footstep] Playing sandStepClip!");
                        audioSrc.PlayOneShot(sandStepClip);
                    }
                    else
                    {
                        Debug.LogWarning("[Footstep] sandStepClip이 할당되지 않았습니다.");
                    }
                }
                // 일반 지면(“Ground”)일 때
                else if (hit.CompareTag("Ground"))
                {
                    if (groundStepClip != null)
                    {
                        Debug.Log("[Footstep] Playing groundStepClip!");
                        audioSrc.PlayOneShot(groundStepClip);
                    }
                    else
                    {
                        Debug.LogWarning("[Footstep] groundStepClip이 할당되지 않았습니다.");
                    }
                }
                // Tag가 “Sand”나 “Ground”가 아닐 때 (추가 지면 종류가 있다면 여기에 분기 추가)
                else
                {
                    Debug.Log($"[Footstep] Detected ground but Tag != Sand/ Ground (Tag: {hit.tag})");
                }
            }
        }
        else
        {
            // 이동이 멈추거나 공중에 있으면 타이머 리셋
            stepTimer = stepInterval;
        }
    }

    // 디버그용: Scene 뷰에서 지면 감지 영역 시각화
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }

    public void Stun()
    {
        Stun(stunDuration);
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        movement.enabled = false;

        yield return new WaitForSeconds(duration);

        movement.enabled = true;
        isStunned = false;
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (isStunned || isStuck || isKeyInput)
            return;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        isKnockBack = true;
        knockbackTimer = duration;
    }

    public override void Bind()
    {
        _binding.Assign<bool>("canJump", () => movement.canJump, v => movement.canJump = (bool)v);
        _binding.Assign<bool>("canLadder", () => interaction.canLadder, v => interaction.canLadder = (bool)v);
        _binding.Assign<bool>("hasCoralStaff", () => combat.hasCoralStaff, v => combat.hasCoralStaff = (bool)v);
    }

    public override void TakeDamage(float value)
    {
        if (isInvincible)
        {
            Debug.Log("무적 상태 중, 데미지 무시됨.");
            return;
        }

        base.TakeDamage(value);
        StartInvincibility();
    }

    public void StartInvincibility()
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        Debug.Log("무적 상태 시작!");

        SpriteRenderer[] weaponRenderers = GameObject
            .FindGameObjectsWithTag("Weapon")
            .Select(go => go.GetComponent<SpriteRenderer>())
            .Where(sr => sr != null)
            .ToArray();

        float elapsedTime = 0f;
        yield return new WaitForSeconds(0.1f);
        while (elapsedTime < invincibleDuration)
        {
            bool visibility = !spriteRenderer.enabled;
            spriteRenderer.enabled = visibility;

            foreach (var weaponRenderer in weaponRenderers)
            {
                weaponRenderer.enabled = visibility;
            }

            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        spriteRenderer.enabled = true;
        foreach (var weaponRenderer in weaponRenderers)
        {
            weaponRenderer.enabled = true;
        }

        isInvincible = false;
        Debug.Log("무적 상태 종료!");
    }

    protected override void ScheduleDestroy()
    {
        Destroy(gameObject, 2f);
    }

    protected override void Death()
    {
        base.Death();
        Instantiate(UIManager.Instance.GameOverCanvasPrefab);
    }

    public void SetBossRound() => movement.isBossRound = true;
}
