using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerVisualEffect : MonoBehaviour
{
    public float duration = 30f;
    private float remainingTime;
    private bool isRunning = false;

    public TextMeshProUGUI timerText;
    public Image overlayEffect;
    public AudioSource urgentSound;

    private Color green = new Color(0.4f, 1f, 0.4f);
    private Color yellow = new Color(1f, 0.92f, 0.3f);
    private Color orange = new Color(1f, 0.5f, 0f);
    private Color purple = new Color(0.8f, 0f, 0.5f);

    private float baseFontSize;


    void OnEnable()
    {
        remainingTime = duration;
        isRunning = true;

        if (urgentSound != null)
            urgentSound.Stop();

        timerText.text = remainingTime.ToString("F2");
        timerText.color = green;

        baseFontSize = timerText.fontSize;

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

        if (remainingTime > 30f)
        {
            timerText.color = green;
            timerText.rectTransform.localScale = Vector3.one;
        }
        else if (remainingTime > 10f)
        {
            timerText.color = yellow;
            timerText.rectTransform.localScale = Vector3.one;
        }
        else // 10초 이하
        {
            timerText.color = orange;

            //텍스트 커졌다 작아짐 (심장박동)
            float pulse = 1f + Mathf.PingPong(Time.time * 1.01f, 0.5f);
            timerText.rectTransform.localScale = new Vector3(pulse, pulse, 1f);

            // 오버레이 깜빡임
            if (overlayEffect != null)
            {
                float alpha = Mathf.PingPong(Time.time * 0.5f, 0.3f);
                overlayEffect.color = new Color(purple.r, purple.g, purple.b, alpha);
            }

            if (urgentSound != null && !urgentSound.isPlaying)
                urgentSound.Play();
        }

        // 시간 종료 처리
        if (remainingTime <= 0f)
        {
            isRunning = false;
            timerText.rectTransform.localScale = Vector3.one;
            HandleTimeout();
        }
    }

    void HandleTimeout()
    {
        Debug.Log("시간 초과!");

        if(overlayEffect != null)
            overlayEffect.color = Color.clear;

        if(timerText != null)
            timerText.color = Color.white;

        if (urgentSound != null)
            urgentSound.Stop();

        Debug.Log("시간 초과! 플레이어 사망.");
     
        Player.Instance.TakeDamage(9999); // 사망 처리
    }
}
