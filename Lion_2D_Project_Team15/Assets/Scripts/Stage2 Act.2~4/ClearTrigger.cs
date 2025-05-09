using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    public bool IsCleared { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsCleared) return;

        if (other.CompareTag("Player"))
        {
            IsCleared = true;
            //Debug.Log("클리어 트리거 도착 - 장애물 스폰 중지 및 제거");

            // ObstacleSpawner 정지
            ObstacleSpawner spawner = Object.FindFirstObjectByType<ObstacleSpawner>();
            if (spawner != null)
                spawner.StopSpawning();

            // 모든 장애물 제거
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject obj in obstacles)
            {
                Destroy(obj);
            }
        }
    }
}
