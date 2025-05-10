using UnityEngine;

public class Monster : Entity
{
    [Header("Monster Stats")]
    public float moveSpeed;
    public float attackPower;
    public float attackCooldown;
    private float lastAttackTime = -999f;

    [Header("Attack Settings")]
    public float attackRange;

    [Header("Knockback Settings")]
    public float knockbackForce = 15f;
    public float knockbackDuration = 0.3f; // 넉백 유지 시간(초)

    [Header("AI 설정")]
    public float patrolSpeed = 2f;
    public float playerDetectRange = 5f;

    [HideInInspector]
    public PatrolArea patrolArea; // 스폰 시 할당

    private Transform player;
    private Rigidbody2D rb;

    // Knockback 상태 관리용
    private bool isKnockback = false;
    private float knockbackTimer = 0f;

    private bool movingRight = true; // 순찰 방향

    // 죽음 상태 플래그
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 'Player' 태그가 붙은 오브젝트 자동 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (isDead) return; // 죽었으면 아무 행동도 하지 않음
        if (player == null) return;

        // 넉백 중이면 이동/공격 불가
        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockback = false;
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= playerDetectRange)
        {
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 멈춤
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
        if (isDead) return; // 죽었으면 행동 금지

        if (patrolArea == null || patrolArea.leftPoint == null || patrolArea.rightPoint == null)
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
        if (isDead) return; // 죽었으면 행동 금지

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        anim.SetBool("Walk", true);
        Flip(direction.x > 0);
    }

    private void Flip(bool faceRight)
    {
        transform.localScale = new Vector3(faceRight ? 1 : -1, 1, 1);
    }

    public void Attack()
    {
        if (isDead) return; // 죽었으면 행동 금지

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            if (anim != null)
                anim.SetTrigger("Attack");

            Debug.Log($"몬스터가 플레이어를 공격! 공격력: {attackPower}");

            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange)
            {
                Debug.Log("플레이어 공격 거리 안!");

                Player target = player.GetComponent<Player>();
                if (target != null)
                {
                    Debug.Log("플레이어에게 데미지 입힘");
                    target.TakeDamage(attackPower);
                }
            }
        }
    }

    public override void TakeDamage(float value)
    {
        if (isDead) return; // 죽었으면 데미지 무시

        base.TakeDamage(value);

        // Knockback 적용
        if (player != null)
        {
            Vector2 hitDirection = (transform.position - player.position).normalized;
            Knockback(hitDirection);
        }
    }

    protected override void Death()
    {
        isDead = true; // 죽음 상태 진입
        base.Death();
    }

    public void Knockback(Vector2 hitDirection)
    {
        if (isDead) return; // 죽었으면 행동 금지

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
