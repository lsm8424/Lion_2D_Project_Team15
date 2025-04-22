using UnityEngine;

/// <summary>
/// 포탈의 동작 방식을 정의하는 열거형
/// </summary>
public enum PortalType
{
    SceneChange,     // 다른 씬으로 이동
    PositionChange   // 같은 씬 내 포탈 위치로 이동
}

/// <summary>
/// 포탈 동작을 제어하는 컴포넌트
/// </summary>
public class Portal : MonoBehaviour
{
    public PortalType portalType;        // 포탈 동작 타입 선택

    [Header("포탈 인덱스 정보")]
    public int portalIndex;              // 현재 포탈의 고유 인덱스
    public int targetPortalIndex;        // 이동할 포탈 인덱스

    [Header("씬 이동용")]
    public string targetSceneName;       // 이동할 씬 이름

    [Header("공통")]
    public Transform targetPortal;       // 도착할 포탈 Transform (오프셋 조정용)

    private void Start()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.RegisterPortal(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        switch (portalType)
        {
            case PortalType.SceneChange:
                if (string.IsNullOrEmpty(targetSceneName))
                {
                    Debug.LogError($"[Portal] TargetSceneName이 비어 있습니다. ({gameObject.name})");
                    return;
                }
                // 씬 이동 요청 (목표 포탈 인덱스 포함)
                StageManager.Instance.TeleportScene(targetSceneName);
                break;

            case PortalType.PositionChange:
                // 같은 씬 내 포탈 이동
                StageManager.Instance.TeleportToPortal(targetPortalIndex);
                break;
        }
    }
}