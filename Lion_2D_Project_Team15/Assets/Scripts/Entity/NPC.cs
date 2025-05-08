using UnityEngine;

public enum SpeakerType { Player, NPC }

[System.Serializable]
public class DialogueLine
{
    public SpeakerType speaker; // 말하는 주체
    public string text; // 대사 내용
}
public class NPC : MonoBehaviour
{
    
    public string NPCName; // NPC 이름
    public DialogueLine[] dialogueLines;
    private int dialogueIndex = 0; // 현재 몇번째 대사인지 추적

    public string EventID;

    // 플레이어가 F 키로 상호작용 시 호출됨
    public void Interact()
    {
        // 수정 필요
        // DialogueManager.Instance.StartDialogue(DialogueCategory.Dialogue, DialogueID);
        if (!string.IsNullOrWhiteSpace(EventID))
            EventManager.Instance.RunEvent(EventID);
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
