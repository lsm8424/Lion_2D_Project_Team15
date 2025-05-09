using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;

    public GameObject xObstaclePrefab;
    public GameObject yObstaclePrefab;

    public Transform[] xSpawnPoints;
    public Transform[] ySpawnPoints;

    public float xSpawnInterval = 3f;
    public float ySpawnInterval = 1f;

    public float gameDuration = 20f; // 제한 시간
    private float timer;

    private bool isGameRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartObstaclePhase();
    }

    private void Update()
    {
        if (!isGameRunning) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Debug.Log("⏰ 제한 시간 종료! 게임 오버");
            isGameRunning = false;
            PlayerDeath();
        }
    }

    public void StartObstaclePhase()
    {
        isGameRunning = true;
        timer = gameDuration;

        InvokeRepeating(nameof(SpawnXObstacle), 1f, xSpawnInterval);
        InvokeRepeating(nameof(SpawnYObstacle), 1f, ySpawnInterval);
    }

    void SpawnXObstacle()
    {
        if (!isGameRunning) return;

        int rand = Random.Range(0, xSpawnPoints.Length);
        Instantiate(xObstaclePrefab, xSpawnPoints[rand].position, Quaternion.identity);
    }

    void SpawnYObstacle()
    {
        if (!isGameRunning) return;

        int rand = Random.Range(0, ySpawnPoints.Length);
        Instantiate(yObstaclePrefab, ySpawnPoints[rand].position, Quaternion.identity);
    }

    public void GameClear()
    {
        if (!isGameRunning) return;

        Debug.Log("플레이어가 클리어 트리거에 도달했습니다. 게임 클리어!");

        isGameRunning = false;

        CancelInvoke(nameof(SpawnXObstacle));
        CancelInvoke(nameof(SpawnYObstacle));

        // 추가 처리: 클리어 효과, 포탈 생성 등
    }

    private void PlayerDeath()
    {
        // Player에게 데미지를 주거나 즉시 사망 처리
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.TakeDamage(player.HP); // 즉사
        }
    }
}
