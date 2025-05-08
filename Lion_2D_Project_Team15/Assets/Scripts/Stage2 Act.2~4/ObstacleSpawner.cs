using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("공통 설정")]
    public GameObject obstaclePrefab;
    public float obstacleSpeed = 5f;
    public float duration = 10f; // 제한 시간

    [Header("스폰 포인트")]
    public Transform[] ySpawnPoints;  // 위에서 아래로
    public Transform[] xSpawnPoints;  // 옆에서 날아옴

    [Header("빈도 설정")]
    public float ySpawnInterval = 1.5f; // 자주
    public float xSpawnInterval = 5f;   // 가끔

    [Header("타겟")]
    public Transform player;
    public GameObject clearTrigger;

    private bool spawning = false;
    private float timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !spawning)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        spawning = true;
        timer = duration;
        StartCoroutine(SpawnY());
        StartCoroutine(SpawnX());

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        ClearTrigger clear = clearTrigger?.GetComponent<ClearTrigger>();
        if (clear != null && !clear.IsCleared)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(9999); // 사망 처리
            }
        }
    }

    IEnumerator SpawnY()
    {
        while (timer > 0)
        {
            Transform spawn = ySpawnPoints[Random.Range(0, ySpawnPoints.Length)];
            SpawnObstacle(spawn.position, Vector2.down);
            yield return new WaitForSeconds(ySpawnInterval);
        }
    }

    IEnumerator SpawnX()
    {
        while (timer > 0)
        {
            Transform spawn = xSpawnPoints[Random.Range(0, xSpawnPoints.Length)];
            Vector2 direction = (spawn.position.x > player.position.x) ? Vector2.left : Vector2.right;
            SpawnObstacle(spawn.position, direction);
            yield return new WaitForSeconds(xSpawnInterval);
        }
    }

    void SpawnObstacle(Vector2 position, Vector2 direction)
    {
        GameObject obj = Instantiate(obstaclePrefab, position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().linearVelocity = direction * obstacleSpeed;
    }
}
