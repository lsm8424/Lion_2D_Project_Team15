using UnityEngine;

public class Player : Entity
{
    // ────────────── Singleton ──────────────

    // Player 인스턴스를 전역에서 접근 가능하도록 static으로 선언
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
        // 각 기능 모듈의 매서드 실행
        movement.HandleMove(); // 이동
        movement.HandleJump(); // 점프
        combat.HandleAttack(); // 기본 공격 (좌클릭)
        combat.HandleSkill(); // 스킬 공격 (우클릭)
        interaction.HandleInteraction(); // F 키 상호작용 (NPC, 아이템 등)
    }
}
