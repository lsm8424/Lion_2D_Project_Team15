using UnityEngine;

public class Stage2_Boss_Audio : MonoBehaviour
{
    public static Stage2_Boss_Audio Instance; // 싱글톤 인스턴스
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 인스턴스 설정
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 가져오기

    }

    private AudioSource audioSource; // 오디오 소스 컴포넌트

    public void PlayOneShot(AudioClip clip, float value)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, audioSource.volume * value); // 사운드 재생
        }
    }

}
