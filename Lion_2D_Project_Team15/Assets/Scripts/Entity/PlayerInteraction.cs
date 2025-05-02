using UnityEngine;
using System.Collections;

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

    public void HandleInteraction()
    {
        // 내용
    }


    private void Update()
    {
        DetectInteractable(); // 항상 감지

        // 스페이스로 사다리 탈출
        if (isOnLadder && Input.GetKeyDown(KeyCode.Space))
        {
            ExitLadder();
            return;
        }

        // F 키 눌렀을 때 상호작용
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ladderJustEntered) return;

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

    private void DetectInteractable()
    {
        Vector2 dir = Player.Instance.movement.facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + dir * 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, interactRange, interactLayerMask);
        Debug.DrawRay(origin, dir * interactRange, Color.yellow);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            // 태그가 상호작용 가능한 종류 중 하나일 때만 메시지 출력
            if (hitObject.CompareTag("NPC") || hitObject.CompareTag("Item") || hitObject.CompareTag("Ladder"))
            {
                Debug.Log("상호작용 가능한 대상입니다: " + hitObject.name);
            }

            currentTarget = hitObject;
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
        if (Time.timeSinceLevelLoad < 1.0f) return;

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

        if (anim != null)
            anim.SetTrigger("ClimbStart");

        Debug.Log("사다리에 올라탐");

        StartCoroutine(ResetLadderJustEntered());
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

    private IEnumerator ResetLadderJustEntered()
    {
        yield return new WaitForSeconds(0.3f);
        ladderJustEntered = false;
    }

    public bool IsOnLadder() => isOnLadder;
    public Ladder GetCurrentLadder() => currentLadder;
    public void ForceExitLadder() => ExitLadder();
}
