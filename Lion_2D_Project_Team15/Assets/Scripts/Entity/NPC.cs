using System;
using UnityEngine;

public enum SpeakerType
{
    Player,
    NPC
}

[System.Serializable]
public class DialogueLine
{
    public SpeakerType speaker; // 말하는 주체
    public string text; // 대사 내용
}

public class NPC : IdentifiableMonoBehavior, IInteractable
{
    public string NPCName; // NPC 이름
    public DialogueLine[] dialogueLines;

    public string EventID;

    public event Action<InteractionType> OnInteracted;

    // 플레이어가 F 키로 상호작용 시 호출됨
    public void Interact()
    {
        // 수정 필요
        // DialogueManager.Instance.StartDialogue(DialogueCategory.Dialogue, DialogueID);
        OnInteracted?.Invoke(InteractionType.Interaction);
        if (!string.IsNullOrWhiteSpace(EventID))
            EventManager.Instance.RunEvent(EventID);

        OnInteracted?.Invoke(InteractionType.Interaction);
    }

    // 플레이어가 F 키를 다시 누르면 호출됨
    //public void AdvanceDialogue()
    //{
    //    DialogueManager.Instance.ProcessPlayerInput();
    //}

    protected virtual void OnDialogueEnd()
    {
        Debug.Log("대화 종료");
        Player.Instance.interaction.EndDialogue();
    }
}
