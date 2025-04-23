using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName; // NPC �̸�
    public string[] dialogues; // ��ȭ ���� �迭
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

        if (dialogueIndex >= dialogues.Length) 
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
        // ���� �ε����� �ش��ϴ� ��� ���
        Debug.Log($"{npcName}: {dialogues[dialogueIndex]}");
    }

    // ��ȭ�� ������ �� ȣ���
    protected virtual void OnDialogueEnd()
    {
        Debug.Log("��ȭ ����");

        //�÷��̾�� ��ȭ ���� �˸�
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerInteraction>()
            .EndDialogue();
    }
}
