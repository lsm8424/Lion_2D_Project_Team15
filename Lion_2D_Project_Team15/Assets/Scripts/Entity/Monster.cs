using UnityEngine;

public class Monster : Entity
{
    [Header("Monster Stats")]
    public float moveSpeed;
    public float attackPower;
    public float attackCooldown; //공격속도
    private float lastAttackTime = -999f; //공격 쿨타임이 바로 가능하도록 하기 위함.게임이 시작되자마자 플레이어가 바로 공격할 수 있도록 하기 위해

    //"마지막 공격 시간"을 일부러 아주 오래 전 시간으로 설정

    [Header("Attack Settings")]
    public float attackRange;

    private Transform player;

    public void Start()
    {
        // 'Player' 태그가 붙은 오브젝트 자동 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    public void Update()
    {
        if (player == null)
            return; //플레이어가 없으면 행동 X

        float distance = Vector3.Distance(transform.position, player.position); //몬스터와 플레이어 사이의 거리 계산

        if (distance > attackRange)
        {
            Move(); //사거리 밖이면 플레이어를 향해 이동
        }
        else
        {
            Attack(); //사거리 이내면 공격 시도
        }
    }

    public override void Move()
    {
        // 플레이어를 향해 이동
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // 플레이어 바라보게 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    public void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown) // 마지막 공격 이후 쿨타임이 지났을 때만 공격 가능
        {
            lastAttackTime = Time.time;

            if (anim != null)
                anim.SetTrigger("Attack");

            Debug.Log($"몬스터가 플레이어를 공격! 공격력: {attackPower}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
