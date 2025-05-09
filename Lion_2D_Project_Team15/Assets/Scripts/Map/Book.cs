using UnityEngine;

public class Book : IdentifiableMonoBehavior, IInteractable
{
    public string EventID; // 카메라 이동과 연결된 이벤트 ID (필요 시 사용)

    public event System.Action<InteractionType> OnInteracted; // 상호작용 이벤트

    private void Awake()
    {
        // 초기화 코드 (필요한 경우)
    }

    public void Interact() { }

    void Start() { }

    void Update() { }
}
