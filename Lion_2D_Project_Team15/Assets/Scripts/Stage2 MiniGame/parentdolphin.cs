using UnityEngine;
using System.Collections;

public class ParentDolphin : NPC
{
    public string dolphinName = "Parent Dolphin";
    private int dialogueIndex = 0;

    public AudioClip targetMelody;
    private AudioSource audioSource;

    private bool isTalking = false;
    private bool isAnswerPhase = false;
    private bool awaitingInput = false;

    // public DialogueLine[] dialogueLines; // 기존 대화 라인들

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!isAnswerPhase)
        {
            dialogueIndex = 0;
            isTalking = true;
            ShowDialogue();
        }
        else if (!awaitingInput)
        {
            StartCoroutine(WaitForAnswerInput());
        }
    }

    public void AdvanceDialogue()
    {
        if (!isTalking)
            return;

        dialogueIndex++;

        if (dialogueIndex >= dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        var line = dialogueLines[dialogueIndex];

        if (line.speaker == SpeakerType.NPC)
        {
            Debug.Log($"{dolphinName}: {line.text}");
        }
        else
        {
            Debug.Log($"Player: {line.text}");
        }
    }

    private void EndDialogue()
    {
        isTalking = false;
        Debug.Log("대화 종료");

        // 멜로디 재생
        PlayTargetMelody();

        // 정답으로 사용할 멜로디 ID 저장
        EventManager.Instance.SetFlag("correctMelody", "babydolphin2");

        // 다음에 다시 상호작용하면 정답 체크로 전환
        isAnswerPhase = true;

        // 대화 종료 처리 (플레이어 상호작용 시스템과 연결)
        Player.Instance.interaction.EndDialogue();
    }

    public void PlayTargetMelody()
    {
        if (audioSource != null && targetMelody != null)
        {
            audioSource.PlayOneShot(targetMelody);
        }
    }

    private IEnumerator WaitForAnswerInput()
    {
        awaitingInput = true;

        Debug.Log("어떤 아기 돌고래가 이 멜로디를 불렀는지 맞춰보세요! (1~3번 숫자키)");

        bool answered = false;
        string selected = "";

        while (!answered)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { selected = "babydolphin1"; answered = true; }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) { selected = "babydolphin2"; answered = true; }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) { selected = "babydolphin3"; answered = true; }

            yield return null;
        }

        // 정답 판정
        if (EventManager.Instance.TryGetFlag("correctMelody", out string correct))
        {
            if (selected == correct)
            {
                Debug.Log("정답입니다!");
                EventManager.Instance.RunEvent("melody_correct");
            }
            else
            {
                Debug.Log("틀렸습니다.");
                EventManager.Instance.RunEvent("melody_wrong");
            }
        }

        awaitingInput = false;
    }
}
