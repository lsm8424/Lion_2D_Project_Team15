using UnityEngine;
using TMPro; // TextMeshPro 사용
using System.Collections; // IEnumerator 사용

public class GameTimer : MonoBehaviour
{
    public float totalTime = 30f; // 전체 제한 시간
    public Transform player;      // 플레이어 참조
    public GameObject targetMap;  // 타이머가 활성화될 맵 (Map5)
    public TextMeshProUGUI timerText; // UI 텍스트 참조

    private float timer;
    private bool isGameOver = false;
    private bool isFlashing = false;

    void Start()
    {
        timer = totalTime;

        // 초기 UI 비활성화
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 플레이어가 타겟 맵에 있는지 확인
        if (!IsPlayerInTargetMap() || isGameOver)
            return;

        // 타이머 UI 활성화
        if (timerText != null && !timerText.gameObject.activeSelf)
            timerText.gameObject.SetActive(true);

        // 타이머 갱신
        timer -= Time.deltaTime;

        // 남은 시간 표시
        UpdateTimerText();

        // 경고 효과 활성화
        if (timer <= 10f && !isFlashing)
        {
            StartCoroutine(FlashTimerText());
        }

        if (timer <= 0)
        {
            OnTimeUp();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt((timer - Mathf.Floor(timer)) * 100); // 밀리초 계산

            timerText.text = $"{seconds:00}:{milliseconds:00}";
        }
    }

    private void OnTimeUp()
    {
        isGameOver = true;
        Debug.Log("시간 초과! 플레이어 사망.");
        if (player != null)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(9999); // 사망 처리
            }
        }
    }

    public void FinalClear()
    {
        isGameOver = true;
        Debug.Log("최종 클리어 트리거 도달! 게임 클리어.");
    }

    private IEnumerator FlashTimerText()
    {
        isFlashing = true;

        while (!isGameOver && timer <= 10f)
        {
            if (timerText != null)
            {
                timerText.color = Color.red;
                yield return new WaitForSeconds(0.5f);

                timerText.color = Color.white;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private bool IsPlayerInTargetMap()
    {
        if (targetMap == null || player == null)
            return false;

        // 타겟 맵의 경계 내부에 플레이어가 있는지 확인
        Bounds mapBounds = targetMap.GetComponent<Collider2D>().bounds;
        return mapBounds.Contains(player.position);
    }
}
