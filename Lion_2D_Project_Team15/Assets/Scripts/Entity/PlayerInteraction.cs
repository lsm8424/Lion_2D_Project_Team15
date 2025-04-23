using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // ��ȣ�ۿ� ������ �Ÿ� (Ray ����)
    public float interactRange = 2f;

    
    private NPC currentNPC = null; // ���� ��ȣ�ۿ� ���� NPC (��ȭ ���)
    private bool isTalking = false; // ��ȭ ������ ����

    public void HandleInteraction()
    {
        // F Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.F))
        {
            // ���� ��ȭ ���̶�� �� ���� ���� �ѱ�
            if (isTalking)
            {
                currentNPC?.AdvanceDialogue(); // null�� �ƴϸ� AdvanceDialogue() ����
            }
            else
            {
                // ��ȭ ���� �ƴϸ� �� Ray�� ���� NPC�� ã��
                RaycastHit hit;

                //�÷��̾� �� �������� interactRange��ŭ Ray �߻�
                if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
                {
                    GameObject target = hit.collider.gameObject;

                    // ���� ������Ʈ�� �±װ� "NPC" ���
                    if (target.CompareTag("NPC"))
                    {
                        currentNPC = target.GetComponent<NPC>(); // NPC ��ũ��Ʈ ��������
                        if (currentNPC != null)
                        {
                            currentNPC.Interact(); // ��ȭ ����
                            isTalking = true; // ��ȭ ���·� ��ȯ
                        }
                    }

                    // ���� ������ ��ȣ�ۿ뵵 �߰� ����
                    
                }
            }
        }
    }

    // ��ȭ ���� �� ȣ��
    public void EndDialogue()
    {
        isTalking = false; // ��ȭ ���� ����
        currentNPC = null; // ��ȭ ��� �ʱ�ȭ
    }
}
