using UnityEngine;

public class Stage2_Boss_NormalAttack : MonoBehaviour
{
    [Header("기본 공격")]
    [SerializeField] private GameObject normalAttackPrefab; // 기본 공격 프리팹
    [SerializeField] private float coolTime; // 공격 쿨타임
    [SerializeField] private float bulletspeed; // 공격 속도
    [SerializeField] private float nockBack; // 넉백 힘
    [SerializeField] private float damage; // 공격력

    [Header("웨이브")]
    [SerializeField] private float waveFrequency; // 웨이브 주기
    [SerializeField] private float waveAmplitude; // 웨이브 진폭

    float delta;
    Vector3 attackDir; // 공격 방향
    GameObject player; // 플레이어 오브젝트

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트 찾기
        delta = coolTime;
    }

    void Update()
    {
        delta -= Time.deltaTime;
        if (delta <= 0)
        {
            Shoot();
            delta = coolTime;
        }
    }

    void Shoot()
    {
        attackDir = player.transform.position - transform.position; // 플레이어 방향 계산
        Vector3 dir = attackDir.normalized; // 방향 벡터 정규화

        GameObject shootObj = Instantiate(normalAttackPrefab, transform.position + new Vector3(dir.x, dir.y, 0), Quaternion.identity);
        shootObj.GetComponent<Stage2_Boss_Bullet>().SetBullet(dir, bulletspeed, nockBack, damage, waveFrequency, waveAmplitude); // 불렛 설정
    }
}
