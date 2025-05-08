using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    // ────────────── Singleton ──────────────

    // Player 인스턴스를 전역에서 접근 가능하도록 static으로 선언

    private bool isStunned = false;
    public float stunDuration = 1f;

    public bool IsStunned => isStunned;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        // 씬에 Player가 이미 존재한다면 현재 오브젝트를 제거
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; // 인스턴스 등록
        }
    }

    // 기능별 모듈 스크립트 참조 (외부에서 Player.Instance.movement 처럼 사용 가능)
    [HideInInspector]
    public PlayerMovement movement;

    [HideInInspector]
    public PlayerCombat combat;

    [HideInInspector]
    public PlayerInteraction interaction;

    private void Start()
    {
        // Player에 붙어있는 기능별 스크립트를 가져옴
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (GameManager.Instance.ShouldWaitForEntity())
            return;

        // 키입력 상태이거나 회오리에 갇혔으면 velocity를 0으로 설정 및 이동 무시
        if (isKeyInput || isStuck)
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            return;
        }

        // 넉백 지속 시간을 줄여주고, 끝나면 이동 잠금 해제
        if (isKnockBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
                isKnockBack = false;
            return; // 이동 입력 무시
        }

        // 각 기능 모듈의 매서드 실행
        movement.HandleMove(); // 이동
        movement.HandleJump(); // 점프
        combat.HandleAttack(); // 기본 공격 (좌클릭)
        combat.HandleSkill(); // 스킬 공격 (우클릭)
        interaction.HandleInteraction(); // F 키 상호작용 (NPC, 아이템 등)
    }

    public void Stun()
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine());
        }
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        //Debug.Log("플레이어가 경직되었습니다!");
        movement.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        movement.enabled = true;
        isStunned = false;
        //Debug.Log("플레이어가 경직에서 회복되었습니다!");
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (isStunned || isStuck || isKeyInput) return; // 회오리 갇힘 상태이거나 키 입력 중이면 넉백 적용 안함

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // 기존 속도 초기화
        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        isKnockBack = true;
        knockbackTimer = duration;

}
