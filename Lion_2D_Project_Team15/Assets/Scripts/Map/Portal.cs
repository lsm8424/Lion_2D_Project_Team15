using UnityEngine;

/// <summary>
/// 포탈의 동작 방식을 정의하는 열거형
/// </summary>
public enum PortalType
{
    SceneChange, // 다른 씬으로 이동
    PositionChange // 같은 씬 내 포탈 위치로 이동
}

/// <summary>
/// 포탈 동작을 제어하는 컴포넌트
/// </summary>
public class Portal : IdentifiableMonoBehavior, IInteractable
{
    public string EventID; // 포탈과 연결된 이벤트 ID (필요 시 사용)

    public event System.Action<InteractionType> OnInteracted; // 상호작용 이벤트

    [Header("포탈 동작 타입")]
    public PortalType portalType; // 포탈 동작 타입 선택

    [Header("포탈 인덱스 정보")]
    public int MapIndex; // 현재 포탈이 속한 씬의 인덱스 (씬 이동 시 사용)
    public int portalIndex; // 현재 포탈의 고유 인덱스

    [Header("위치 이동용")]
    public int targetPortalIndex; // 이동할 포탈 인덱스

    [Header("씬 이동용")]
    public string targetSceneName; // 이동할 씬 이름

    [Header("공통")]
    public Transform targetPortal; // 도착할 포탈 Transform (오프셋 조정용)
    public Sprite closeSprite; // 포탈 닫힘 상태 스프라이트
    public Sprite openSprite; // 포탈 열림 상태 스프라이트

    private void Start()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.RegisterPortal(this);
        }
        ClosePortal();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        switch (portalType)
        {
            case PortalType.SceneChange:
                if (string.IsNullOrEmpty(targetSceneName))
                {
                    Debug.LogError($"[Portal] TargetSceneName이 비어 있습니다. ({gameObject.name})");
                    return;
                }
                // 씬 이동 요청 (목표 포탈 인덱스 포함)
                StageManager.Instance.TeleportScene(targetSceneName, targetPortalIndex);
                break;

            case PortalType.PositionChange:
                // 같은 씬 내 포탈 이동
                StageManager.Instance.TeleportToPortal(targetPortalIndex);
                break;
        }

        OpenDoor(); // 포탈 열기
        Invoke("ClosePortal", 1f); // 1초 후 포탈 닫기
    }

    void OpenDoor()
    {
        // 포탈을 여는 로직 (예: 스프라이트 변경)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = openSprite; // 포탈 열림 상태로 변경
    }

    void ClosePortal()
    {
        // 포탈을 닫는 로직 (예: 스프라이트 변경)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeSprite; // 포탈 열림 상태로 변경
    }

    void AnimatorFalse()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false; // 애니메이터 컴포넌트 비활성화
        }
    }
}
