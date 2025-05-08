using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Stun(); // 플레이어 경직
            }
        }
    }
}
