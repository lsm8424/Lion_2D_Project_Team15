using UnityEngine;

public class Stage2_Boss_Bullet : MonoBehaviour
{
    private float speed; // 총알 속도
    private Vector3 direction;
    private Quaternion rotation; // 총알 회전

    //웨이브
    private float waveFrequency; // 물결 주기
    private float waveAmplitude; // 물결 효과의 진폭
    private Vector3 perpendicular; // 웨이브 방향 (진행 방향에 수직)
    private float time;

    private float damage; // 공격력
    private float nockBack; //넉백

    void Start()
    {
        Destroy(gameObject, 3f); // 5초 후 총알 삭제
    }

    void Update()
    {
        time += Time.deltaTime; // 시간 증가

        Vector3 waveOffset = perpendicular * Mathf.Sin(time * waveFrequency) * waveAmplitude;
        transform.position += (direction * speed * Time.deltaTime) + (waveOffset * Time.deltaTime);
    }

    public void SetBullet(Vector2 dir, float speed, float force, float damage, float waveFreauency, float waveAmplitude)
    {
        direction = dir.normalized; // 방향 벡터 정규화
        this.speed = speed; // 속도 설정
        this.nockBack = force; // 넉백 힘 설정
        this.damage = damage; // 공격력 설정
        this.waveFrequency = waveFreauency; // 웨이브 주기 설정
        this.waveAmplitude = waveAmplitude; // 웨이브 진폭 설정
        perpendicular = Vector3.Cross(direction, Vector3.forward);  // 진행 방향에 수직인 벡터
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<move>().isStuck || collision.GetComponent<move>().isKeyInput)  // 회오리 갇힘 상태이거나 키 입력 중이면 넉백 적용 안함
            {
                Destroy(gameObject); // 플레이어와 충돌 시 삭제
                return;
            }

            //if (Player.Instance.isStuck || Player.Instance.isKeyInput)  // 회오리 갇힘 상태이거나 키 입력 중이면 넉백 적용 안함
            //{
            //    Destroy(gameObject); // 플레이어와 충돌 시 삭제
            //    return;
            //}

            //플레이어 함수에 넉백 적용
            collision.GetComponent<move>().ApplyKnockback(direction, nockBack, 0.1f); // 플레이어 넉백
            Debug.Log("보스 기본 공격");

            //플레이어 데미지
            //Player.Instance.ApplyKnockback(direction, nockBack, 0.1f); // 플레이어 넉백
            //Player.Instance.TakeDamage(damage);

            Destroy(gameObject); // 플레이어와 충돌 시 삭제
        }
    }

}
