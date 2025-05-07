using UnityEngine;

public class Stage2_Boss_LoachSkill : MonoBehaviour
{
    private int waveCount; // 몹 개수
    private float moveSpeed; // 이동속도
    private BoxCollider2D cd;   //콜라이더
    private float angle; // 회전 각도
    private float damage; // 공격력

    [SerializeField] private GameObject wavePrefab; // 돌진 지렁이 프리팹

    void Start()
    {
        cd = GetComponent<BoxCollider2D>();
        SpawnLoach(waveCount);
        SetCDandRot();

        Destroy(gameObject, 1f); // 1초 후에 삭제
    }

    void Update()
    {
        colliderOffset();
    }

    public void SetLoach(float angle, float speed, int count, float damage)
    {
        this.angle = angle;
        moveSpeed = speed;
        waveCount = count;
        this.damage = damage;
    }

    void SpawnLoach(int count)
    {
        if (count <= 0)
            return;

        int halfCount = count / 2; // 몹 개수 반으로 나누기

        bool flip = Mathf.Abs(angle) > 90;

        //짝수 일때 위치 조정
        if (count % 2 == 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject loach = Instantiate(wavePrefab);
                loach.transform.parent = transform;
                loach.transform.position = transform.position + new Vector3(0, 0.5f + i - halfCount, 0); // 몹 위치 조정
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GameObject loach = Instantiate(wavePrefab);
                loach.transform.parent = transform;
                loach.transform.position = transform.position + new Vector3(0, i - halfCount, 0); // 몹 위치 조정
            }
        }
    }

    void SetCDandRot()
    {
        cd.size = new Vector2(10, waveCount);
        cd.offset = new Vector2(5, 0); // 콜라이더 offset
        transform.rotation = Quaternion.Euler(0, 0, angle); // 회전
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Player.Instance.TakeDamage(damage); // 플레이어 데미지
            Debug.Log("플레이어가 파동에 맞음!");

            Destroy(gameObject); // 몹이 플레이어와 충돌하면 삭제
        }
    }

    private void colliderOffset()
    {
        // 콜라이더의 offset을 진행 방향으로 증가
        Vector2 offset = cd.offset;

        //1초에 10만큼 이동
        offset.x += 10 * Time.deltaTime;

        cd.offset = offset;

    }

}
