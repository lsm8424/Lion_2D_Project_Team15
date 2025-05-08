using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [Header("공격 설정")]
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log($"장애물이 플레이어에게 {damage} 데미지를 입힘");
                player.TakeDamage(damage);
                player.Stun(); // 경직 처리 (Player 스크립트에 Stun 함수 존재해야 함)
            }

            // 필요시 자기 자신 제거
            Destroy(gameObject);
        }
    }
}
