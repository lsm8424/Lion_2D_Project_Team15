using UnityEngine;

public class InteractIndicatorHandler : MonoBehaviour
{
    [Header("말풍선 프리팹")]
    public GameObject indicatorPrefab;

    [Header("표시 위치 (transform1)")]
    public Transform indicatorAnchor;

    [Header("플레이어 위쪽일 때 사용될 표시 위치 (transform2, 선택 사항)")]
    public Transform alternativeAnchor;

    private GameObject currentInstance;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("[InteractIndicatorHandler] Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (Player.Instance == null || Player.Instance.interaction == null)
            return;

        if (Player.Instance.interaction.IsOnLadder())
        {
            Hide(); // 올라탄 상태라면 강제로 말풍선 제거
        }
    }

    public void Show()
    {
        if (Player.Instance == null || Player.Instance.interaction == null)
        {
            Debug.LogWarning("[InteractIndicatorHandler] Player 혹은 interaction이 null입니다.");
            return;
        }

        if (indicatorPrefab == null || indicatorAnchor == null || playerTransform == null)
        {
            Debug.LogWarning($"[{name}] 프리팹, 앵커, 플레이어 중 누락된 항목이 있습니다.");
            return;
        }

        // 사다리에 올라탄 경우 표시하지 않음
        if (Player.Instance.interaction.IsOnLadder())
        {
            Debug.Log($"[InteractIndicatorHandler] 플레이어가 사다리 위에 있음 → 말풍선 비활성화");
            Hide();
            return;
        }

        // 어느 위치에 말풍선을 띄울지 결정
        Transform targetAnchor = indicatorAnchor;

        if (alternativeAnchor != null && playerTransform.position.y > indicatorAnchor.position.y)
        {
            targetAnchor = alternativeAnchor;
        }

        if (currentInstance == null)
        {
            currentInstance = Instantiate(
                indicatorPrefab,
                targetAnchor.position,
                Quaternion.identity
            );
            currentInstance.transform.SetParent(targetAnchor);
        }
        else
        {
            currentInstance.transform.SetParent(targetAnchor);
            currentInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void Hide()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }
    }
}
