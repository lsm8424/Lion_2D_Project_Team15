using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("장비 관련")]
    public GameObject coralStaffInHand;
    public float coralStaffAttackPower = 15f;

    [Header("공격 설정")]
    public float attackPower;
    public float attackCooldown;
    private float lastAttackTime = -999f;
    public bool canAttack = true;

    [Header("스킬 설정")]
    public float skillCooldown;
    private float lastSkillTime = -999f;

    [Header("발사체 설정")]
    public GameObject coralProjectilePrefab;
    public Transform firePoint;

    private Animator anim;
    public bool hasCoralStaff = false;
    public bool hasStick = false; // 막대기 보유 여부를 여기에 세팅

    private PlayerMovement playerMovement;

    [Header("무기 연결")]
    public Sword sword;      // 검
    public GameObject stick; // 막대기 오브젝트

    [Header("사운드")]
    public AudioClip punchClip;       // 주먹 소리
    public AudioClip stickSwingClip;  // 막대기 휘두르는 소리
    private AudioSource audioSrc;

    private void Awake()
    {
        // AudioSource 준비
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null)
            audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.playOnAwake = false;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Coral Staff 초기 비활성화
        if (coralStaffInHand != null)
            coralStaffInHand.SetActive(false);

        // stick도 없으면 비활성화
        if (stick != null)
            stick.SetActive(false);
    }

    public void HandleAttack()
    {
        if (!canAttack)
            return;

        float h = Input.GetAxisRaw("Horizontal");
        playerMovement.FlipByDirection(h);

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            if (playerMovement != null)
                playerMovement.isAttacking = true;

            if (anim != null)
                anim.SetTrigger("Attack");

            // ─────────────────────────────────────
            // 1) 검을 가지고 있으면 검 공격 처리
            if (hasCoralStaff && sword != null)
            {
                sword.TriggerAttack();
                
            }
            // 2) 막대기를 가지고 있으면 막대기 휘두르기
            else if (hasStick)
            {
                // 막대기 애니메이션 트리거(예: "StickAttack" 파라미터)
                if (anim != null)
                    anim.SetTrigger("StickAttack");

                // 막대기 사운드 재생
                if (stickSwingClip != null)
                {
                    audioSrc.PlayOneShot(stickSwingClip);
                }
                else
                {
                    Debug.LogWarning("[PlayerCombat] stickSwingClip이 할당되지 않았습니다.");
                }
            }
            // 3) 둘 다 없으면 기본 주먹 공격
            else
            {
                // 주먹 공격 사운드
                if (punchClip != null)
                    audioSrc.PlayOneShot(punchClip);
                else
                    Debug.LogWarning("[PlayerCombat] punchClip이 할당되지 않았습니다.");
            }
            // ─────────────────────────────────────

            // 타격 판정 (예시는 범위를 모두 동일하게 처리)
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2.5f);
            foreach (Collider2D col in hits)
            {
                if (col.CompareTag("Monster"))
                {
                    Entity monster = col.GetComponent<Entity>();
                    if (monster != null)
                    {
                        monster.TakeDamage(attackPower);
                    }
                }
            }

            Invoke(nameof(ResetAttackState), 0.7f);
        }
    }

    private void ResetAttackState()
    {
        if (playerMovement != null)
            playerMovement.isAttacking = false;
    }

    public void HandleSkill()
    {
        if (!hasCoralStaff)
            return;

        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCooldown)
        {
            lastSkillTime = Time.time;

            if (anim != null)
                anim.SetTrigger("Skill");

            Debug.Log("CoralStaff 스킬 발사!");
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        if (coralProjectilePrefab == null || firePoint == null)
            return;

        GameObject projectile = Instantiate(
            coralProjectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Rigidbody2D rb2 = projectile.GetComponent<Rigidbody2D>();
        CoralProjectile cp = projectile.GetComponent<CoralProjectile>();
        if (cp != null) cp.damage = coralStaffAttackPower;
        if (rb2 != null)
        {
            Vector3 mouseInput = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 shootdir = mouseInput - transform.position;
            shootdir.z = 0f;
            rb2.linearVelocity = shootdir.normalized * 10f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 attackCenter = (Vector2)transform.position + Vector2.right * direction * 1.0f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackCenter, 1.5f);
    }
}
