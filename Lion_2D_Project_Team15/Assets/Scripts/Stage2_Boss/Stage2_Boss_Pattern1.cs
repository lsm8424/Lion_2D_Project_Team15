using UnityEngine;

public class Stage2_Boss_Pattern1 : MonoBehaviour
{
    [Header("Pattern 1 (원형 공격)")]
    [SerializeField] private GameObject bulletPrefab; // 총알 프리팹
    [SerializeField] private float bulletSpeed; // 총알 속도
    [SerializeField] private float bulletCount; // 총알 개수
    [SerializeField] private float coolTime; // 쿨타임

    private float delta;

    void Start()
    {
        
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
        float oneangle = 360f / bulletCount; // 각도 간격

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * oneangle;
            float radian = angle * Mathf.Deg2Rad;

            // 방향 계산 (XY 평면에서)
            Vector3 direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f);

            GameObject bullet = Instantiate(bulletPrefab, transform.position + direction.normalized, Quaternion.identity);
            bullet.GetComponent<Stage2_Boss_Pattern1_Bullet>().Initialize(bulletSpeed, direction); // 해당 방향으로 발사
        }
    }
}
