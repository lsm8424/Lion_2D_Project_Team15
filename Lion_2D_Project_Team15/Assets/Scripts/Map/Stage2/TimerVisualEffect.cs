using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerVisualEffect : MonoBehaviour
{
    public float duration = 60f;
    private float remainingTime;
    private bool isRunning = false;

    public TextMeshProUGUI timerText;
    public Image overlayEffect;
    public AudioSource urgentSound;

    // 색상 정의
    private Color green = new Color(0.4f, 1f, 0.4f);          // 연한 초록
    private Color yellow = new Color(1f, 0.92f, 0.3f);        // 노랑
    private Color orange = new Color(1f, 0.5f, 0f);           // 주황
    private Color purple = new Color(0.6f, 0f, 1f);           // 보라

    void OnEnable()
    {
        remainingTime = duration;
        isRunning = true;

        if (urgentSound != null)
            urgentSound.Stop();

        timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        timerText.color = green;

        // 오버레이 초기화
        if (overlayEffect != null)
            overlayEffect.color = new Color(purple.r, purple.g, purple.b, 0f);
    }

    void Update()
    {
        if (GameManager.Instance.DialogueTimeScale == 0 || GameManager.Instance.EntityTimeScale == 0)
        {
            isRunning = false;
            return;
        }

        if (!isRunning) return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(0f, remainingTime);

        timerText.text = remainingTime.ToString("F2");

        // 텍스트 색상 단계별 적용
        if (remainingTime > 30f)
        {
            timerText.color = green;
        }
        else if (remainingTime > 10f)
        {
            timerText.color = yellow;
        }
        else // 10초 이하
        {
            timerText.color = orange;

            // 보라색 오버레이 깜빡임 효과
            if (overlayEffect != null)
            {
                float alpha = Mathf.PingPong(Time.time * 4f, 0.3f);
                overlayEffect.color = new Color(purple.r, purple.g, purple.b, alpha);
            }

            // 긴급 효과음 재생
            if (urgentSound != null && !urgentSound.isPlaying)
                urgentSound.Play();
        }

        // 시간 초과
        if (remainingTime <= 0f)
        {
            isRunning = false;
            HandleTimeout();
        }
    }

    void HandleTimeout()
    {
        Debug.Log("시간 초과!");
        // 실패 처리 or 이벤트 연결
    }
}
