using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("��ȣ�ۿ� ����")]
    public float interactRange; // ��ȣ�ۿ� �Ÿ�

    private NPC currentNPC = null;       // ���� ��ȭ ���� NPC
    private bool isTalking = false;      // ��ȭ ������ ����

    private Ladder currentLadder = null; // ���� �ö�ź ��ٸ�
    private bool isOnLadder = false;     // ��ٸ� ���� ����

    private Rigidbody2D rb;              // 2D Rigidbody
    private Animator anim;               // �ִϸ��̼�

    public LayerMask interactLayerMask; // Inspector���� ������ �� �ְ� ����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F Ű ����! ��ȣ�ۿ� �õ� ��");

            if (isTalking)
            {
                currentNPC?.AdvanceDialogue(); // ��ȭ �ѱ��
                return;
            }

            if (isOnLadder)
            {
                ExitLadder(); // ��ٸ� ��������
                return;
            }

            // ������ NPC �Ǵ� Ladder Ž���� Raycast (2D ����) ������
            Vector2 direction = transform.right;
            Vector2 origin = (Vector2)transform.position + direction * 0.1f;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactRange, interactLayerMask);
            Debug.DrawRay(origin, direction * interactRange, Color.red, 1f);


            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;
                Debug.Log("Ray�� ������ ������ϴ�: " + hit.collider.name);

                if (target.CompareTag("NPC"))
                {
                    currentNPC = target.GetComponent<NPC>();
                    if (currentNPC != null)
                    {
                        currentNPC.Interact(); // ��ȭ ����
                        isTalking = true;
                    }
                }
                else if (target.CompareTag("Ladder"))
                {
                    currentLadder = target.GetComponent<Ladder>();
                    if (currentLadder != null)
                    {
                        EnterLadder();
                    }
                }
            }
        }
    }

    // ��ȭ ���� �� ȣ���
    public void EndDialogue()
    {
        isTalking = false;
        currentNPC = null;
    }

    // ��ٸ� ž�� ó��
    private void EnterLadder()
    {
        isOnLadder = true;
        Player.Instance.movement.enabled = false;
        rb.gravityScale = 0f;

        if (anim != null)
            anim.SetTrigger("ClimbStart");

        Debug.Log("��ٸ��� �ö�Ž");
    }

    // ��ٸ� Ż�� ó��
    private void ExitLadder()
    {
        isOnLadder = false;
        currentLadder = null;
        Player.Instance.movement.enabled = true;
        rb.gravityScale = 1f;

        if (anim != null)
            anim.SetTrigger("ClimbEnd");

        Debug.Log("��ٸ����� ������");
    }

    // �ܺ� ���ٿ�: ���� ��ٸ� ����
    public bool IsOnLadder() => isOnLadder;
    public Ladder GetCurrentLadder() => currentLadder;
    public void ForceExitLadder() => ExitLadder();
}
