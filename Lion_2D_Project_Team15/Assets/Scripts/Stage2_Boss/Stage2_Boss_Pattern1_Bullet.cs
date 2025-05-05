using UnityEngine;

public class Stage2_Boss_Pattern1_Bullet : MonoBehaviour
{
    private float speed;
    private Vector3 direction;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(float speed, Vector3 direction)
    {
        this.speed = speed;
        this.direction = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 처리
            Destroy(gameObject); // 총알 삭제
        }

    }

}
