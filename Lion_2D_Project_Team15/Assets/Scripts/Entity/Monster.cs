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

    private Transform player;
    private Rigidbody2D rb;

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
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 멈춤
            Attack();
        }
    }

    public override void Move()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // X축만 이동 (Y축은 중력 유지)
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // 스프라이트 방향 반전 처리
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            if (anim != null)
                anim.SetTrigger("Attack"); // 널 체크 나중에 에니메이션 추가되면 변경

            Debug.Log($"몬스터가 플레이어를 공격! 공격력: {attackPower}");

            // 피격 판정
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
