using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    public bool IsCleared { get; private set; } = false;

    public bool isFinalTrigger = false; // 이 트리거가 최종 트리거인지 여부

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsCleared) return;

        if (other.CompareTag("Player"))
        {
            IsCleared = true;
            Debug.Log($"{gameObject.name} 클리어 트리거 도달!");

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

            // 최종 트리거 처리
            if (isFinalTrigger)
            {
                GameTimer timer = Object.FindFirstObjectByType<GameTimer>();
                if (timer != null)
                {
                    timer.FinalClear();
                }
            }
        }
    }
}
