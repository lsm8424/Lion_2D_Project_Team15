using UnityEngine;

public class CameraPos : IdentifiableMonoBehavior, IInteractable
{
    public string EventID; // 카메라 이동과 연결된 이벤트 ID (필요 시 사용)

    public event System.Action<InteractionType> OnInteracted; // 상호작용 이벤트

    void Start() { }

    void Update() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("[TriggerZone] 플레이어가 트리거 영역에 진입");

        OnInteracted?.Invoke(InteractionType.OnTriggerEnter);
    }
}
