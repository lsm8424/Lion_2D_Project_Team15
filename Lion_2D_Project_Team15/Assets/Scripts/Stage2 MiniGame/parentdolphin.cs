using UnityEngine;

public class ParentDolphin : NPC
{
    public string dolphinName = "Parent Dolphin"; // 대화 이름
    private int dialogueIndex = 0;                 // 대화 인덱스

    public AudioClip targetMelody;                 // 목표 멜로디
    private AudioSource audioSource;

    private bool isTalking = false; // 대화 중인지

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        dialogueIndex = 0;
        isTalking = true;
        ShowDialogue();
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

        // 마지막에 멜로디 재생
        PlayTargetMelody();

        // 플레이어 쪽 대화 상태 종료
        Player.Instance.interaction.EndDialogue();
    }

    public void PlayTargetMelody()
    {
        if (audioSource != null && targetMelody != null)
        {
            audioSource.PlayOneShot(targetMelody);
        }
    }
}
