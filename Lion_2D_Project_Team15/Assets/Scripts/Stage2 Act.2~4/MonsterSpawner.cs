using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("몬스터 스폰 설정")]
    public GameObject monsterPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int maxSpawnCount = 3;


    [Header("스폰 구역 설정")]
    public PatrolArea[] patrolAreas; // 각 스폰 포인트마다 구역 지정

    private float timer;
    private int currentSpawnCount = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentSpawnCount < maxSpawnCount)
        {
            SpawnMonster();
            timer = 0f;
        }
    }

    private void SpawnMonster()
    {
        if (spawnPoints.Length == 0 || monsterPrefab == null)
        {
            Debug.LogWarning("스폰 지점이나 프리팹이 설정되지 않았습니다.");
            return;
        }

        int idx = Random.Range(0, spawnPoints.Length);
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject monster = Instantiate(monsterPrefab, point.position, Quaternion.identity);
        currentSpawnCount++;

        // 몬스터가 죽으면 currentSpawnCount를 줄이도록 이벤트 연결
        Entity entity = monster.GetComponent<Entity>();
        if (entity != null)
        {
            entity.OnDeath += () => currentSpawnCount--;
        }

        //Debug.Log($"몬스터 스폰 완료: {monster.name}");

        // 몬스터에 PatrolArea 할당
        Monster monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null && patrolAreas.Length > idx)
        {
            monsterScript.patrolArea = patrolAreas[idx];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.3f);
        }

        // 구역 시각화
        Gizmos.color = Color.cyan;
        if (patrolAreas != null)
        {
            foreach (var area in patrolAreas)
            {
                if (area.leftPoint != null && area.rightPoint != null)
                {
                    Gizmos.DrawLine(area.leftPoint.position, area.rightPoint.position);
                }
            }
        }
    }
}
