using UnityEngine;
using UnityEngine.UI;

public enum MonsterType
{
    Normal,
    Dasher,
    Stunner,
    Ranged
}

public class Monster : Entity
{
    [Header("Monster 타입")]
    public MonsterType monsterType = MonsterType.Normal;

    [Header("Splitting Settings")]
    public bool splitsOnDeath = true;         // 분열 기능 켜/끄
    public GameObject splitPrefab;           // 분열할 몬스터 프리팹 (자기 자신 또는 작은 버전)
    public float splitOffset = 0.5f;         // 분열된 몬스터가 스폰될 좌우 거리

    [Header("Monster Stats")]
    public float moveSpeed;
    public float attackPower;
    public float attackCooldown;
    private float lastAttackTime = -999f;

    [Header("Attack Settings")]
    public float attackRange;

    [Header("원거리 공격 설정")]
    public GameObject inkPrefab;
    public Transform firePoint;

    [Header("Knockback Settings")]
    public float knockbackForce = 15f;
    public float knockbackDuration = 0.3f;

    [Header("AI 설정")]
    public float patrolSpeed = 2f;
    public float playerDetectRange = 5f;

    [Header("돌진 공격 설정")]
    public float dashForce = 20f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 2f;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float lastDashTime = -999f;

    [Header("돌진 조건")]
    public float dashStartDistance = 3f;
    public float dashMinDistance = 1f;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private Image healthBarFill;
    private GameObject healthBarInstance;

    [HideInInspector]
    public PatrolArea patrolArea;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isKnockback = false;
    private float knockbackTimer = 0f;

    private bool movingRight = true;
    private bool isDead = false;
    private bool IsStopped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(
                healthBarPrefab,
                transform.position + Vector3.up * 1.2f,
                Quaternion.identity
            );
            healthBarFill = healthBarInstance.transform.Find("Background/Fill").GetComponent<Image>();
        }
    }

    private void LateUpdate()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.transform.position = transform.position + Vector3.up * 1.2f;
            healthBarInstance.transform.localScale = Vector3.one;
        }
    }

    private void Update()
    {
        // 상태 정지 처리
        if (GameManager.Instance.ShouldWaitForEntity())
        {
            anim.speed = 0;
            IsStopped = true;
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }
        else if (IsStopped)
        {
            anim.speed = 1f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            IsStopped = false;
        }

        if (isDead || player == null)
            return;

        // 넉백 처리
        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockback = false;
            return;
        }

        // 돌진 중 처리
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 원거리 몬스터 처리
        if (monsterType == MonsterType.Ranged)
        {
            if (distanceToPlayer <= attackRange)
                Attack();
            else
                MoveTowardsPlayer();
            return;
        }

        // 일반, 돌진 몬스터 행동
        if (distanceToPlayer <= playerDetectRange)
        {
            // 돌진 몬스터 우선 처리
            if (monsterType == MonsterType.Dasher)
            {
                if (Time.time >= lastDashTime + dashCooldown
                    && distanceToPlayer <= dashStartDistance
                    && distanceToPlayer >= dashMinDistance)
                {
                    DashAttack();
                    return;
                }
            }

            // 추격 vs 근접 공격
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                Attack();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (isDead || patrolArea == null || patrolArea.leftPoint == null || patrolArea.rightPoint == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 left = patrolArea.leftPoint.position;
        Vector2 right = patrolArea.rightPoint.position;

        if (movingRight)
        {
            rb.linearVelocity = new Vector2(patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x >= right.x)
            {
                movingRight = false;
                Flip(false);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x <= left.x)
            {
                movingRight = true;
                Flip(true);
            }
        }

        anim.SetBool("Walk", true);
    }

    private void MoveTowardsPlayer()
    {
        if (isDead) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        anim.SetBool("Walk", true);
        Flip(direction.x > 0);
    }

    private void Flip(bool faceRight)
    {
        transform.localScale = new Vector3(faceRight ? 1 : -1, 1, 1);
    }

    private void DashAttack()
    {
        isDashing = true;
        dashTimer = dashDuration;
        lastDashTime = Time.time;

        // 플레이어 방향 계산
        Vector2 dir = (player.position - transform.position).normalized;

        // 대시 방향으로 바라보기
        Flip(dir.x > 0);

        // 기존 속도 초기화
        rb.linearVelocity = Vector2.zero;

        // 로컬 스케일 기반 대시 힘 적용 (항상 바라보는 방향으로 대시)
        Vector2 dashVector = new Vector2(transform.localScale.x * dashForce, 0f);
        rb.AddForce(dashVector, ForceMode2D.Impulse);

        if (anim != null)
            anim.SetTrigger("Dash");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDashing) return;
        if (other.CompareTag("Player"))
        {
            Player target = other.GetComponent<Player>();
            if (target != null)
            {
                target.TakeDamage(attackPower);
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (monsterType != MonsterType.Dasher || !isDashing) return;
        if (collision.collider.CompareTag("Player"))
        {
            Player target = collision.collider.GetComponent<Player>();
            if (target != null)
            {
                target.TakeDamage(attackPower);
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void Attack()
    {
        if (isDead) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");

            if (monsterType == MonsterType.Ranged && inkPrefab != null && firePoint != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                GameObject ink = Instantiate(inkPrefab, firePoint.position, Quaternion.identity);
                InkProjectile proj = ink.GetComponent<InkProjectile>();
                if (proj != null)
                    proj.Initialize(dir);
                return;
            }

            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= attackRange)
            {
                Player target = player.GetComponent<Player>();
                if (target != null)
                {
                    target.TakeDamage(attackPower);
                    if (monsterType == MonsterType.Stunner)
                        target.Stun(0.5f);
                }
            }
        }
    }

    public override void TakeDamage(float value)
    {
        if (isDead) return;
        base.TakeDamage(value);
        UpdateHealthBar();

        if (player != null)
        {
            Vector2 hitDirection = (transform.position - player.position).normalized;
            Knockback(hitDirection);
        }
    }

    protected override void Death()
    {
        // 분열 처리 (한 번만)
        if (splitsOnDeath && splitPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                float dir = (i == 0) ? -1f : 1f;
                Vector3 spawnPos = transform.position + Vector3.right * dir * splitOffset;

                GameObject clone = Instantiate(splitPrefab, spawnPos, Quaternion.identity);
                Monster m = clone.GetComponent<Monster>();
                if (m != null)
                {
                    // 체력 절반 세팅
                    m.maxHP = this.maxHP * 0.5f;
                    m.HP = m.maxHP;

                    // 복제체는 더 이상 분열하지 않도록 꺼줌
                    m.splitsOnDeath = false;
                }
            }
        }

        // 원래 사망 처리
        isDead = true;
        base.Death();
        if (healthBarInstance != null)
            Destroy(healthBarInstance);
    }


    public void Knockback(Vector2 hitDirection)
    {
        if (isDead) return;
        Vector2 force = new Vector2(hitDirection.x, 0.1f).normalized * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);
        isKnockback = true;
        knockbackTimer = knockbackDuration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (patrolArea != null && patrolArea.leftPoint != null && patrolArea.rightPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(patrolArea.leftPoint.position, patrolArea.rightPoint.position);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float ratio = HP / maxHP;
            healthBarFill.fillAmount = ratio;
        }
    }
}
