using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange; // 상호작용 거리
    public LayerMask interactLayerMask; // 상호작용 레이어

    private NPC currentNPC = null;
    private bool isTalking = false;

    private Ladder currentLadder = null;
    private bool isOnLadder = false;

    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isTalking)
            {
                currentNPC?.AdvanceDialogue();
                return;
            }

            if (isOnLadder)
            {
                ExitLadder();
                return;
            }

            // ─── Ray 쏴서 상호작용 감지 ───
            Vector2 direction = transform.right;
            Vector2 origin = (Vector2)transform.position + direction * 0.1f;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactRange, interactLayerMask);
            Debug.DrawRay(origin, direction * interactRange, Color.red, 1f);

            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;
                Debug.Log("Ray가 맞춘 오브젝트: " + hit.collider.name);

                if (target.CompareTag("NPC"))
                {
                    currentNPC = target.GetComponent<NPC>();
                    if (currentNPC != null)
                    {
                        currentNPC.Interact();
                        isTalking = true;
                    }
                }
                else if (target.GetComponent<ParentDolphin>() != null) // 태그와 상관없이 컴포넌트로 감지
                {
                    currentNPC = target.GetComponent<ParentDolphin>();
                    if (currentNPC != null)
                    {
                        currentNPC.Interact();
                        isTalking = true;
                    }
                }
                else if (target.CompareTag("Episode2_babydolphin_1"))
                {
                    babydolphin baby = target.GetComponent<babydolphin>();
                    if (baby != null)
                    {
                        baby.Select();
                    }
                }
                else if (target.CompareTag("Item")) // 아이템 줍기
                {
                    PickupItem(target);
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

    // 아이템 습득 처리
    private void PickupItem(GameObject item)
    {
        
        if (Time.timeSinceLevelLoad < 1.0f)
            return;


        Debug.Log("아이템을 획득했습니다: " + item.name);

        Destroy(item);

    }

    public void EndDialogue()
    {
        isTalking = false;
        currentNPC = null;
    }

    private void EnterLadder()
    {
        isOnLadder = true;
        Player.Instance.movement.enabled = false;
        rb.gravityScale = 0f;

        if (anim != null)
            anim.SetTrigger("ClimbStart");

        Debug.Log("사다리에 올라탐");
    }

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

    public bool IsOnLadder() => isOnLadder;
    public Ladder GetCurrentLadder() => currentLadder;
    public void ForceExitLadder() => ExitLadder();
}
