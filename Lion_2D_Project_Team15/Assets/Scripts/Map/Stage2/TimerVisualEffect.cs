using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerVisualEffect : MonoBehaviour
{
    // 전체 제한 시간 (기본값: 60초)
    public float duration = 60f;

    // 현재 남은 시간
    private float remainingTime;

    // 시간이 흐를지 여부를 체크하는 변수
    private bool isRunning = false;

    // 타이머 텍스트 UI
    public Text timerText;

    // 화면 위에 깔리는 붉은 오버레이 (깜빡임 연출용)
    public Image overlayEffect;

    // 긴급 상황 효과음 (삐삐삐 등)
    public AudioSource urgentSound;

    // Canvas 또는 GameObject가 활성화될 때 호출됨
    void OnEnable()
    {
        // 남은 시간을 초기화
        remainingTime = duration;

        // 타이머 시작
        isRunning = true;

        // 효과음이 이전에 재생 중이었다면 멈춤
        if (urgentSound != null)
            urgentSound.Stop();

        // 타이머 텍스트 초기화
        timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        timerText.color = Color.white;
    }

    // 매 프레임마다 호출
    void Update()
    {
        // 타이머가 비활성 상태면 아무것도 하지 않음
        if (!isRunning) return;

        // 시간 감소
        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(0f, remainingTime); // 0 미만으로 떨어지지 않도록

        // 텍스트 UI 업데이트
        timerText.text = Mathf.CeilToInt(remainingTime).ToString();

        // 남은 시간이 10초 이하일 경우 연출 추가
        if (remainingTime <= 10f)
        {
            float intensity = 1f - (remainingTime / 10f); // 남은 시간에 따라 강도 증가

            // 텍스트 색상이 점점 빨개짐
            timerText.color = Color.Lerp(Color.white, Color.red, intensity);

            // 붉은색 오버레이가 깜빡이는 연출
            if (overlayEffect != null)
            {
                float alpha = Mathf.PingPong(Time.time * 4f, 0.3f); // 알파값을 위아래 반복
                overlayEffect.color = new Color(1f, 0f, 0f, alpha); // 반투명 붉은색
            }

            // 긴박한 효과음이 없을 경우 재생
            if (urgentSound != null && !urgentSound.isPlaying)
                urgentSound.Play();
        }

        // 시간이 0초가 되면 타이머 종료 처리
        if (remainingTime <= 0f)
        {
            isRunning = false;
            HandleTimeout(); // 시간 초과시 실행할 함수
        }
    }

    // 시간 초과 시 실행되는 함수
    void HandleTimeout()
    {
        Debug.Log("시간 초과!");
        // 여기서 실패 처리, 씬 이동, 리셋 등 원하는 로직 작성
    }

}
