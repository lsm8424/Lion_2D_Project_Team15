using UnityEngine;

public enum SpeakerType { Player, NPC }

[System.Serializable]
public class DalogueLine
{
    public SpeakerType speaker; // ���ϴ� ��ü
    public string text; // ��� ����
}
public class NPC : MonoBehaviour
{
    
    public string NPCName; // NPC �̸�
    public DalogueLine[] dialogueLines;
    private int dialogueIndex = 0; // ���� ���° ������� ����


    // �÷��̾ F Ű�� ��ȣ�ۿ� �� ȣ���
    public void Interact()
    {
        dialogueIndex = 0; // ��� �ε��� �ʱ�ȭ
        ShowDialogue(); // ù ��° ��� ���
    }


    // �÷��̾ F Ű�� �ٽ� ������ ȣ���
    public void AdvanceDialogue()
    {
        dialogueIndex++; // ���� ���� �̵�

        if (dialogueIndex >= dialogueLines.Length) 
        {
            OnDialogueEnd(); // ��簡 ������ ��� ó��
        }
        else
        {
            ShowDialogue(); // ���� ��� ���
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
        Debug.Log("��ȭ ����");
        Player.Instance.interaction.EndDialogue();
    }
}
