using UnityEngine;

public class Stage2_Boss_Pattern1_Bullet : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private float damage;

    private void Start()
    {
        // 총알이 일정 시간 후에 삭제되도록 설정
        Destroy(gameObject, 3f); // 3초 후에 삭제
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(float speed, Vector3 direction, float damage)
    {
        this.speed = speed;
        this.direction = direction.normalized;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<move>().isStuck || collision.GetComponent<move>().isKeyInput) return;

            //회오리 순간이동 상태거나 키입력 상태일 때는 return
            //if (Player.Instance.isStuck || Player.Instance.isKeyInput)return;

            Debug.Log("보스 패턴1 공격");

            // 플레이어와 충돌 시 처리
            //Player.Instance.TakeDamage(damage); // 플레이어에게 데미지 주기

            Destroy(gameObject); // 총알 삭제
        }

    }

}
