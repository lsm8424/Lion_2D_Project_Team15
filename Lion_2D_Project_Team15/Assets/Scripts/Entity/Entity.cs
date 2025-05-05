using UnityEngine;

public class Entity : IdentifiableMonoBehavior
{
    [Header("스탯")]
    public float HP = 100f;           // 현재 체력
    public float maxHP = 100f;      // 최대 체력 (필요 시 UI에 활용)

    [Header("컴포넌트")]
    public Animator anim;             // 애니메이션 연결

    // ▶ 데미지 받기
    public virtual void TakeDamage(float value)
    {
        HP -= value;
        Debug.Log($"{gameObject.name}이(가) {value} 데미지를 입었습니다. (남은 체력: {HP})");

        if (HP <= 0)
        {
            Death();
        }
    }

    // 이동 함수 (자식 클래스에서 override)
    public virtual void Move()
    {
        // 공통 이동이 필요 없다면 비워둠
    }

    // 사망 처리
    protected virtual void Death()
    {
        Debug.Log($"{gameObject.name}이(가) 사망했습니다.");

        if (anim != null)
        {
            anim.SetTrigger("Death"); // 널 체크 나중에 에니메이션 추가되면 변경
        }

        // 사망 후 제거
        Destroy(gameObject, 1.5f); // 1.5초 후 삭제 (애니메이션 시간 고려)
    }
}
