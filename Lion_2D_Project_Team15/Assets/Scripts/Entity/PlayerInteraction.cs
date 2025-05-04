using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange = 1.5f;
    public LayerMask interactLayerMask;

    private GameObject currentTarget = null;

    private NPC currentNPC = null;
    private bool isTalking = false;

    public Ladder currentLadder { get; private set; }
    private bool isOnLadder = false;

    private Rigidbody2D rb;
    private Animator anim;

    private bool ladderJustEntered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectInteractable(); // 항상 감지

        if (isOnLadder)
        {
            float vertical = Input.GetAxisRaw("Vertical"); // W/S 키 입력 감지

            if (anim != null)
            {
                anim.SetBool("Climb", true); // 사다리 위에 있는 동안 Climb 상태 유지

                if (vertical == 0) // 움직이지 않으면
                {
                    anim.speed = 0; // 애니메이션 일시정지
                }
                else // 움직이면
                {
                    anim.speed = 1; // 애니메이션 재생
                }
            }
        }

        // 스페이스로 사다리 탈출
        if (isOnLadder && Input.GetKeyDown(KeyCode.Space))
        {
            ExitLadder();
            return;
        }

        // F 키 눌렀을 때 상호작용
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ladderJustEntered)
                return;

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

            if (currentTarget != null)
            {
                TryInteract(currentTarget);
            }
        }
    }

    public void HandleInteraction()
    {
        // 내용
    }

    private void DetectInteractable()
    {
        Vector2 dir = Player.Instance.movement.facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + dir * 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, interactRange, interactLayerMask);
        Debug.DrawRay(origin, dir * interactRange, Color.yellow);

        if (hit.collider != null)
        {
            if (currentTarget != hit.collider.gameObject)
            {
                Debug.Log("상호작용할 수 있는 Object입니다: " + hit.collider.name);
            }
            currentTarget = hit.collider.gameObject;
        }
        else
        {
            currentTarget = null;
        }
    }


    private void TryInteract(GameObject target)
    {
        if (target.CompareTag("NPC"))
        {
            currentNPC = target.GetComponent<NPC>();
            if (currentNPC != null)
            {
                currentNPC.Interact();
                isTalking = true;
            }
        }
        else if (target.CompareTag("Item"))
        {
            PickupItem(target);
        }
        else if (target.CompareTag("Ladder"))
        {
            currentLadder = target.GetComponent<Ladder>();
            if (currentLadder != null)
                EnterLadder();
        }
    }

    private void PickupItem(GameObject item)
    {
        if (Time.timeSinceLevelLoad < 1.0f)
            return;

        Debug.Log("아이템을 획득했습니다: " + item.name);

        if (item.name.Contains("CoralStaff"))
        {
            Player.Instance.combat.hasCoralStaff = true;
            Player.Instance.combat.coralStaffInHand.SetActive(true);
            Debug.Log("Coral Staff를 획득했습니다!");
        }

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
        ladderJustEntered = true;

        Player.Instance.movement.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        Debug.Log("사다리에 올라탐");

        StartCoroutine(ResetLadderJustEntered());
    }

    public void ExitLadder()
    {
        isOnLadder = false;
        currentLadder = null;

        Player.Instance.movement.enabled = true;
        rb.gravityScale = 1f;

        if (anim != null)
        {
            anim.SetBool("Climb", false);
            anim.speed = 1f; // 애니메이션 속도를 기본값으로 리셋
        }

        Debug.Log("사다리에서 내려옴");
    }

    private IEnumerator ResetLadderJustEntered()
    {
        yield return new WaitForSeconds(0.3f);
        ladderJustEntered = false;
    }

    public bool IsOnLadder() => isOnLadder;

    public Ladder GetCurrentLadder() => currentLadder;

    public void ForceExitLadder() => ExitLadder();
}
