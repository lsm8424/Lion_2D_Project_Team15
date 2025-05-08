using UnityEngine;

public class Stage2_Boss_WaveSkill : MonoBehaviour
{
    private int waveCount; // 몹 개수
    private float moveSpeed; // 이동속도
    private BoxCollider2D cd;   //콜라이더
    private float angle; // 회전 각도
    private float damage; // 공격력
    private float nockBackForce; // 넉백 힘    
    private Vector2 nockBackDir; // 넉백 방향

    [SerializeField] private GameObject wavePrefab; // 웨이브 프리팹
    private bool crash = false; // 충돌 여부
    private Vector3 startPos; // 시작 위치

    void Start()
    {
        cd = GetComponent<BoxCollider2D>();
        SpawnWave(waveCount);
        SetCDandRot();
    }

    void Update()
    {
        if (crash)
            return;

        DistanceCrash();

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); // 몹 이동
    }

    public void SetWave(float angle, float speed, int count, float nockbackforce, float damage, Vector3 startpos)
    {
        this.angle = angle;
        moveSpeed = speed;
        waveCount = count;
        nockBackForce = nockbackforce * count; //웨이브 갯수에 따라 넉백 힘 증가
        this.damage = damage;
        startPos = startpos; // 시작 위치
    }

    void SpawnWave(int count)
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

                SpriteRenderer[] sr = loach.GetComponentsInChildren<SpriteRenderer>();
                for (int j = 0; j < sr.Length; j++)
                {
                    sr[j].flipY = flip; // 몹 flip
                }
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GameObject loach = Instantiate(wavePrefab);
                loach.transform.parent = transform;
                loach.transform.position = transform.position + new Vector3(0, i - halfCount, 0); // 몹 위치 조정

                SpriteRenderer[] sr = loach.GetComponentsInChildren<SpriteRenderer>();
                for (int j = 0; j < sr.Length; j++)
                {
                    sr[j].flipY = flip; // 몹 flip
                }
            }
        }
    }

    void SetCDandRot()
    {
        cd.size = new Vector2(1, waveCount);
        transform.rotation = Quaternion.Euler(0, 0, angle); // 회전
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (crash)
            return;

        if (collision.CompareTag("Player"))
        {
            PlayerCrash();

            nockBackDir = (collision.transform.position - transform.position).normalized; // 넉백 방향
            collision.GetComponent<move>().ApplyKnockback(nockBackDir, nockBackForce, 0.1f); // 플레이어 넉백
            //Player.Instance.ApplyKnockback(nockBackDir, nockBackForce, 0.1f); // 플레이어 넉백
            //Player.Instance.TakeDamage(damage); // 플레이어 데미지
            Debug.Log("플레이어가 파동에 맞음!");

        }
    }

    private void PlayerCrash()
    {
        crash = true; // 충돌 여부

        AnimatorTrigger[] waveAnimator = GetComponentsInChildren<AnimatorTrigger>();

        foreach (var animator in waveAnimator)
        {
            animator.TriggerCrash(); // 웨이브 애니메이션 트리거
        }
    }

    private void DistanceCrash()
    {
        float distance = (transform.position - startPos).magnitude; // 파동 거리

        if (distance > 20)
        {
            crash = true; // 충돌 여부

            AnimatorTrigger[] waveAnimator = GetComponentsInChildren<AnimatorTrigger>();

            foreach (var animator in waveAnimator)
            {
                animator.TriggerCrash(); // 웨이브 애니메이션 트리거
            }
        }
    }

}
