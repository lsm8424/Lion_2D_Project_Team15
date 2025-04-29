using System;
using UnityEngine;

public class babydolphin : MonoBehaviour
{
    public AudioClip myMelody; // 새끼 돌고래만의 고유한 멜로디
    public AudioSource audioSource; // AudioSource를 추가해서 제어

    private void Start()
    {
        // 자신에게 AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = myMelody;
        audioSource.spatialBlend = 1f; // 3D 사운드로 설정
        audioSource.loop = true; // 근처에 있는 동안 계속 재생하도록 루프 켜기
        audioSource.playOnAwake = false; // 시작할 때 자동 재생 끄기
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play(); // 플레이어가 근처로 오면 멜로디 재생
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop(); // 플레이어가 없어지면 멜로디 멈춤
        }
    }

    public void Select()
    {
        PuzzleManager.Instance.CheckAnswer(this); // 퍼즐 매니저에 내 정보를 전달해서 정답인지 확인
    }
 
}
