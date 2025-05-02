using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("장비 관련")]
    public GameObject coralStaffInHand; // 손에 들려줄 Coral Staff 오브젝트
    public float coralStaffAttackPower = 15f; // Coral Staff 고유 공격력

    [Header("공격 설정")]
    public float attackPower;
    public float attackCooldown;
    private float lastAttackTime = -999f;

    [Header("스킬 설정")]
    public float skillCooldown;
    private float lastSkillTime = -999f;

    [Header("발사체 설정")]
    public GameObject coralProjectilePrefab; // 생성할 발사체 프리팹
    public Transform firePoint; // 발사 위치 (플레이어 위치나 손 위치)

    private Animator anim;
    public bool hasCoralStaff = false;

    private void Start()
    {
        anim = GetComponent<Animator>();

        // 게임 시작 시 Coral Staff는 비활성화 (획득 전까지 숨김)
        if (coralStaffInHand != null)
            coralStaffInHand.SetActive(false);
    }

    public void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            if (anim != null)
                anim.SetTrigger("Attack"); // 널 체크 나중에 에니메이션 추가되면 변경

            Debug.Log("기본 공격!");

            // 공격 범위 내 몬스터 찾기
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f); // 1.5f: 공격 범위

            foreach (Collider2D col in hits)
            {
                Debug.Log("충돌 감지한 오브젝트: " + col.name);

                if (col.CompareTag("Monster"))
                {
                    Debug.Log("몬스터 감지! 데미지 입힘");

                    Entity monster = col.GetComponent<Entity>();
                    if (monster != null)
                    {
                        monster.TakeDamage(attackPower);
                    }
                }
            }
        }
    }

    public void HandleSkill()
    {
        if (!hasCoralStaff)
            return; // CoralStaff 없으면 스킬 못씀

        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCooldown)
        {
            lastSkillTime = Time.time;

            if (anim != null)
                anim.SetTrigger("Skill"); // 널 체크 나중에 에니메이션 추가되면 변경

            Debug.Log("CoralStaff 스킬 발사!");

            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        if (coralProjectilePrefab == null || firePoint == null)
            return;

        GameObject projectile = Instantiate(coralProjectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        CoralProjectile cp = projectile.GetComponent<CoralProjectile>();

        if (cp != null)
        {
            cp.damage = coralStaffAttackPower;
        }

        if (rb != null)
        {
            // 방향 결정
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 shootDir = new Vector2(direction, 0f); // x방향으로만 발사
            rb.linearVelocity = shootDir * 10f;
        }
    }


}
