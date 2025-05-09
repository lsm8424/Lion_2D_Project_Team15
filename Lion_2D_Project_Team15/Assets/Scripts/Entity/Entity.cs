using UnityEngine;
using System;

public class Entity : IdentifiableMonoBehavior
{
    [Header("스탯")]
    public float HP = 100f;
    public float maxHP = 100f;

    [Header("컴포넌트")]
    public Animator anim;

    // 정적 참조 (MonsterKillManager)
    public static MonsterKillManager killManager;

    // 사망 이벤트
    public event Action OnDeath;

    // 데미지 받기
    public virtual void TakeDamage(float value)
    {
        // 무적 상태인 경우 데미지를 무시 (자식 클래스에서 정의됨)
        if (this is Player player && player.IsInvincible)
        {
            Debug.Log($"{gameObject.name}은 무적 상태입니다. 데미지를 무시합니다.");
            return;
        }

        HP -= value;
        Debug.Log($"{gameObject.name}이(가) {value} 데미지를 입었습니다. (남은 체력: {HP})");

        if (HP <= 0)
        {
            Death();
        }
    }


    // 이동 (자식 클래스에서 필요 시 재정의)
    public virtual void Move()
    {
        // 기본 이동 없음
    }

    // 사망 처리
    protected virtual void Death()
    {
        Debug.Log($"{gameObject.name}이(가) 사망했습니다.");

        // 외부 이벤트 알림
        OnDeath?.Invoke();

        // 애니메이션 재생
        if (anim != null)
        {
            anim.SetTrigger("Death");
        }

        // 킬 카운트 등록
        killManager?.RegisterKill();

        // 오브젝트 삭제 (0.5초 후)
        Destroy(gameObject, 0.5f);
    }
}
