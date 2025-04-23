using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName; // NPC 이름
    public string[] dialogues; // 대화 내용 배열
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

        if (dialogueIndex >= dialogues.Length) 
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
        // 현재 인덱스에 해당하는 대사 출력
        Debug.Log($"{npcName}: {dialogues[dialogueIndex]}");
    }

    // 대화가 끝났을 때 호출됨
    protected virtual void OnDialogueEnd()
    {
        Debug.Log("대화 종료");

        //플레이어에게 대화 종료 알림
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerInteraction>()
            .EndDialogue();
    }
}
