using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    [TextArea]
    public string[] dialogues;

    protected int currentDialogueIndex = 0;
    protected bool isTalking = false;

    public virtual void Interact()
    {
        if (!isTalking)
        {
            isTalking = true;
            ShowDialogue();
        }
        else
        {
            AdvanceDialogue();
        }
    }

    protected virtual void ShowDialogue()
    {
        Debug.Log($"{npcName}: {dialogues[currentDialogueIndex]}");
    }

    protected virtual void AdvanceDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex >= dialogues.Length)
        {
            isTalking = false;
            currentDialogueIndex = 0;
            OnDialogueEnd();
        }
        else
        {
            ShowDialogue();
        }
    }

    protected virtual void OnDialogueEnd()
    {
        // 대사 끝나고 퀘스트 주거나 이벤트 트리거 등
    }
}
