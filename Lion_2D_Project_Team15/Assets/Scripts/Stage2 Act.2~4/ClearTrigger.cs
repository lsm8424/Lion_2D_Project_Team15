using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    public bool IsCleared { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsCleared = true;
            Debug.Log("클리어 트리거 작동: 플레이어가 도착함");
        }
    }
}
