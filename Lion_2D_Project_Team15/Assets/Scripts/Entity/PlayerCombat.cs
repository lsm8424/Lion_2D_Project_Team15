using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackPower; // 기본 공격의 데미지
    public float attackCooldown; // 기본 공격 쿨타임
    public float skillCooldown; // 스킬 쿨타임

    public float attackRange = 1.5f; // 공격 범위

    private float lastAttackTime = -999f; // 마지막 공격 시간 (처음부터 공격 가능하게 초기화)
    private float lastSkillTime = -999f; // 마지막 스킬 사용 시간

    private Animator anim; // 애니메이터 컴포넌트

    private void Start()
    {
        anim = GetComponent<Animator>(); // 이 오브젝트의 Animator 가져오기
    }

    public void HandleAttack()
    {
        // 마우스 좌클릭 & 쿨타임 체크
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // 공격 시간 갱신
            if (anim != null)
                anim.SetTrigger("Attack"); // 애니메이션 실행

            Debug.Log("기본 공격! 공격력: " + attackPower);
        }
    }

    public void HandleSkill()
    {
        // 마우스 우클릭 & 쿨타임 체크
        if (Input.GetMouseButtonDown(1) && Time.time >= lastSkillTime + skillCooldown)
        {
            lastSkillTime = Time.time; // 스킬 시간 갱신
            if (anim != null)
                anim.SetTrigger("Skill");

            Debug.Log("스킬 사용!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Scence 뷰에서 선택했을 때 빨간 원으로 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
