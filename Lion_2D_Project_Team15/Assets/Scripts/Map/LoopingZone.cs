using UnityEngine;

public class LoopingZone : MonoBehaviour
{
    [SerializeField] Transform respawnPoint; // 플레이어가 이동될 위치

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 충돌했을 때
        {
            collision.transform.position = respawnPoint.position; // 새로운 위치로 이동
        }
    }
}
