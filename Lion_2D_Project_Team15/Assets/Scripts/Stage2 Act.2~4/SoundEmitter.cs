using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public GameObject soundParticlePrefab;
    public Transform[] waypoints;         // 경로 웨이포인트 배열
    public float spawnInterval = 1.5f;
    public float particleLifetime = 3f;

    public Transform player;           // 플레이어 Transform 참조
    public float forwardOffset = 1.5f; // 플레이어 앞쪽 오프셋 (거리)

    private float timer;

    void Update()
    {
        // 1. 플레이어 앞쪽으로 위치 갱신
        if (player != null)
        {
            // 플레이어의 진행 방향(오른쪽: 1, 왼쪽: -1)
            float dir = Mathf.Sign(player.localScale.x);
            Vector3 offset = Vector3.right * dir * forwardOffset;
            transform.position = player.position + offset;
        }

        // 2. 파티클 생성 타이머
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnParticle();
            timer = 0f;
        }
    }

    void SpawnParticle()
    {
        GameObject particle = Instantiate(soundParticlePrefab, transform.position, Quaternion.identity);
        SoundParticle sp = particle.GetComponent<SoundParticle>();
        if (sp != null && waypoints != null && waypoints.Length > 0)
        {
            sp.SetWaypoints(waypoints);
        }
        Destroy(particle, particleLifetime);
    }
}
