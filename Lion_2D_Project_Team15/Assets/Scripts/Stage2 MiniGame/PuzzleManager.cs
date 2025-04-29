using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    public ParentDolphin parentDolphin;
    public GameObject coralStaffPrefab; // 산호 지팡이 프리팹 추가
    public Transform rewardSpawnPoint;  // 보상 생성 위치

    public int remainingTries = 3;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckAnswer(babydolphin selectedBaby)
    {
        if (selectedBaby.myMelody == parentDolphin.targetMelody)
        {
            Debug.Log("정답! 퍼즐 클리어!");
            GiveReward(); // 보상 지급
        }
        else
        {
            remainingTries--;
            Debug.Log($"틀렸습니다! 남은 시도: {remainingTries}");

            if (remainingTries <= 0)
            {
                Debug.Log("3번 실패! 퍼즐 리셋");
                ResetPuzzle();
            }
        }
    }

    private void ResetPuzzle()
    {
        remainingTries = 3;
        parentDolphin.PlayTargetMelody();
    }

    private void GiveReward()
    {
        if (coralStaffPrefab != null && rewardSpawnPoint != null)
        {
            Instantiate(coralStaffPrefab, rewardSpawnPoint.position, Quaternion.identity);
            Debug.Log("산호 지팡이 지급!");
        }
        else
        {
            Debug.LogWarning("코랄 스태프 프리팹이나 스폰 위치가 설정되지 않았습니다!");
        }
    }
}
