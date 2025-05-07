using UnityEngine;

public class MonsterKillManager : MonoBehaviour
{
    [Header("목표 설정")]
    public int killTarget = 10; // 목표 마릿수
    private int currentKillCount = 0; // 현재 처치 수

    [Header("스폰 제어")]
    public MonsterSpawner[] spawnersToStop; // 여러 스포너를 배열로 처리

    [Header("포탈 생성")]
    public GameObject portalPrefab;        // 생성할 포탈 프리팹
    public Transform portalSpawnPoint;     // 포탈 생성 위치

    private bool goalReached = false;

    private void Start()
    {
        // 전역 접근용 등록
        Entity.killManager = this;
    }

    public void RegisterKill()
    {
        if (goalReached) return;

        currentKillCount++;
        Debug.Log($"몬스터 처치: {currentKillCount}/{killTarget}");

        if (currentKillCount >= killTarget)
        {
            goalReached = true;
            Debug.Log("목표 달성! 다음 스테이지로 넘어갑니다.");

            // 모든 스포너 비활성화
            foreach (var spawner in spawnersToStop)
            {
                if (spawner != null)
                    spawner.enabled = false;
            }

            // 포탈 생성
            if (portalPrefab != null && portalSpawnPoint != null)
            {
                Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("포탈 프리팹 또는 생성 위치가 지정되지 않았습니다.");
            }
        }
    }
}
