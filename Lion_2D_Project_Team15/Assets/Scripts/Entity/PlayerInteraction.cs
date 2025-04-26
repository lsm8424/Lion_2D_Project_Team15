using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange; // 상호작용 거리

    private NPC currentNPC = null;       // 현재 대화 중인 NPC
    private bool isTalking = false;      // 대화 중인지 여부

    private Ladder currentLadder = null; // 현재 올라탄 사다리
    private bool isOnLadder = false;     // 사다리 상태 여부

    private Rigidbody2D rb;              // 2D Rigidbody
    private Animator anim;               // 애니메이션

    public LayerMask interactLayerMask; // Inspector에서 설정할 수 있게 만듦

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F 키 눌림! 상호작용 시도 중");

            if (isTalking)
            {
                currentNPC?.AdvanceDialogue(); // 대화 넘기기
                return;
            }

            if (isOnLadder)
            {
                ExitLadder(); // 사다리 내려오기
                return;
            }

            // ─── NPC 또는 Ladder 탐지용 Raycast (2D 전용) ───
            Vector2 direction = transform.right;
            Vector2 origin = (Vector2)transform.position + direction * 0.1f;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactRange, interactLayerMask);
            Debug.DrawRay(origin, direction * interactRange, Color.red, 1f);


            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;
                Debug.Log("Ray가 뭔가를 맞췄습니다: " + hit.collider.name);

                if (target.CompareTag("NPC"))
                {
                    currentNPC = target.GetComponent<NPC>();
                    if (currentNPC != null)
                    {
                        currentNPC.Interact(); // 대화 시작
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

    // 대화 종료 시 호출됨
    public void EndDialogue()
    {
        isTalking = false;
        currentNPC = null;
    }

    // 사다리 탑승 처리
    private void EnterLadder()
    {
        isOnLadder = true;
        Player.Instance.movement.enabled = false;
        rb.gravityScale = 0f;

        if (anim != null)
            anim.SetTrigger("ClimbStart");

        Debug.Log("사다리에 올라탐");
    }

    // 사다리 탈출 처리
    private void ExitLadder()
    {
        isOnLadder = false;
        currentLadder = null;
        Player.Instance.movement.enabled = true;
        rb.gravityScale = 1f;

        if (anim != null)
            anim.SetTrigger("ClimbEnd");

        Debug.Log("사다리에서 내려옴");
    }

    // 외부 접근용: 현재 사다리 상태
    public bool IsOnLadder() => isOnLadder;
    public Ladder GetCurrentLadder() => currentLadder;
    public void ForceExitLadder() => ExitLadder();
}
