using UnityEngine;

public class MonsterTriggerSpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject[] monsterPrefabs;
    public Transform[] spawnPoints;

    [Header("스폰 간격 조정")]
    public float positionOffsetRange = 0.5f; // 오프셋 범위 설정 (X축 기준)

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            hasSpawned = true;

            foreach (var point in spawnPoints)
            {
                foreach (var prefab in monsterPrefabs)
                {
                    if (prefab != null && point != null)
                    {
                        // 스폰 위치에 약간의 X축 랜덤 오프셋 추가
                        Vector3 offset = new Vector3(Random.Range(-positionOffsetRange, positionOffsetRange), 0f, 0f);
                        Instantiate(prefab, point.position + offset, Quaternion.identity);
                        Debug.Log($"{prefab.name} 몬스터가 {point.name} 위치에 스폰됨");
                    }
                }
            }
        }
    }
}
