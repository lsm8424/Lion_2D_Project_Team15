using UnityEngine;

public class Stage2_Boss_LoachSkill : MonoBehaviour
{
    private int loachCount; // 몹 개수
    private float moveSpeed; // 이동속도
    private BoxCollider2D cd;   //콜라이더
    private float angle; // 회전 각도
    private float damage; // 공격력

    [SerializeField] private GameObject loachPrefab; // 돌진 지렁이 프리팹

    void Start()
    {
        cd = GetComponent<BoxCollider2D>();
        SpawnLoach(loachCount);
        SetCDandRot();

        Destroy(gameObject, 3f); // 2초 후에 삭제
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); // 몹 이동
    }

    public void SetLoach(float angle, float speed, int count, float damage)
    {
        this.angle = angle;
        moveSpeed = speed;
        loachCount = count;
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
                GameObject loach = Instantiate(loachPrefab);
                loach.transform.parent = transform;
                loach.transform.position = transform.position + new Vector3(0, 0.5f + i - halfCount, 0); // 몹 위치 조정
                loach.GetComponent<SpriteRenderer>().flipY = flip; // 몹 flip
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GameObject loach = Instantiate(loachPrefab);
                loach.transform.parent = transform;
                loach.transform.position = transform.position + new Vector3(0, i - halfCount, 0); // 몹 위치 조정
                loach.GetComponent<SpriteRenderer>().flipY = flip; // 몹 flip
            }
        }
    }

    void SetCDandRot()
    {
        cd.size = new Vector2(4.5f, loachCount);
        transform.rotation = Quaternion.Euler(0, 0, angle); // 회전
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Player.Instance.TakeDamage(damage); // 플레이어 데미지

            Destroy(gameObject); // 몹이 플레이어와 충돌하면 삭제
        }
    }

}
