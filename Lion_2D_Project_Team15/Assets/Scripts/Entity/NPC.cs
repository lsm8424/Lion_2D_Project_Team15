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


    // 플레이어가 F 키로 상호작용 시 호출됨
    public void Interact()
    {
        dialogueIndex = 0; // 대사 인덱스 초기화
        ShowDialogue(); // 첫 번째 대사 출력
    }


    // 플레이어가 F 키를 다시 누르면 호출됨
    public void AdvanceDialogue()
    {
        dialogueIndex++; // 다음 대사로 이동

        if (dialogueIndex >= dialogueLines.Length) 
        {
            OnDialogueEnd(); // 대사가 끝났을 경우 처리
        }
        else
        {
            ShowDialogue(); // 다음 대사 출력
        }

    }

    private void ShowDialogue()
    {
        var line = dialogueLines[dialogueIndex];

        if (line.speaker == SpeakerType.NPC)
        {
            Debug.Log($"{NPCName}: {line.text}");
        }
        else
        {
            Debug.Log($"Player: {line.text}");
        }
    }

    protected virtual void OnDialogueEnd()
    {
        Debug.Log("대화 종료");
        Player.Instance.interaction.EndDialogue();
    }
}
