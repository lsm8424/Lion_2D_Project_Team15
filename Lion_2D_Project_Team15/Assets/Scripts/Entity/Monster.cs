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
    private bool isDashing = false;
    private float dashTimer = 0f;

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

    private bool isKnockback = false;
    private float knockbackTimer = 0f;

    private bool movingRight = true;
    private bool isDead = false;
    bool IsStopped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float ratio = HP / maxHP;
            healthBarFill.fillAmount = ratio;
        }
    }

    private void Update()
    {
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

        if (isDead || player == null) return;

        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockback = false;
            return;
        }

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

        if (monsterType == MonsterType.Ranged)
        {
            if (distanceToPlayer <= attackRange)
            {
                Attack(); // 가까우면 발사
            }
            else
            {
                MoveTowardsPlayer(); // 멀면 추적
            }
            return;
        }


        if (distanceToPlayer <= playerDetectRange)
        {
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                if (monsterType == MonsterType.Dasher && Time.time >= lastAttackTime + attackCooldown)
                {
                    DashAttack();
                }
                else
                {
                    Attack();
                }
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
        lastAttackTime = Time.time;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(dir.x * dashForce, 0f), ForceMode2D.Impulse);

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
                Debug.Log("돌진으로 플레이어 타격!");
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 감지됨: " + collision.collider.name);

        if (monsterType != MonsterType.Dasher) return;
        if (!isDashing) return;

        if (collision.collider.CompareTag("Player"))
        {
            Player target = collision.collider.GetComponent<Player>();
            if (target != null)
            {
                target.TakeDamage(attackPower);
                Debug.Log("돌진 중 플레이어 충돌로 데미지 입힘");
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

            if (anim != null)
                anim.SetTrigger("Attack");

            if (monsterType == MonsterType.Ranged && inkPrefab != null && firePoint != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                GameObject ink = Instantiate(inkPrefab, firePoint.position, Quaternion.identity);
                InkProjectile proj = ink.GetComponent<InkProjectile>();
                if (proj != null)
                {
                    proj.Initialize(dir);
                }
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
                    {
                        target.Stun(0.5f);
                    }
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
}
