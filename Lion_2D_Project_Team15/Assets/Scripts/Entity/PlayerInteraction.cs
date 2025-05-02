using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange;            // 상호작용 거리
    public LayerMask interactLayerMask;    // 상호작용 레이어

    private NPC currentNPC = null;
    private bool isTalking = false;

    private Ladder currentLadder = null;
    private bool isOnLadder = false;

    private Rigidbody2D rb;
    private Animator anim;

    private bool ladderJustEntered = false; // 중복 상호작용 방지용

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Player.cs Update()에서 호출
    public void HandleInteraction()
    {
        // 사다리 타고 있을 때 스페이스바로 탈출
        if (isOnLadder && Input.GetKeyDown(KeyCode.Space))
        {
            ExitLadder();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // 진입 직후 곧바로 탈출 방지
            if (ladderJustEntered)
                return;

            // 대화 중일 때
            if (isTalking)
            {
                currentNPC?.AdvanceDialogue();
                return;
            }

            // 사다리 상태에서 F키 누르면 탈출
            if (isOnLadder)
            {
                ExitLadder();
                return;
            }

            // Raycast로 상호작용 대상 감지
            Vector2 dir = Player.Instance.movement.facingRight ? Vector2.right : Vector2.left;
            Vector2 origin = (Vector2)transform.position + dir * 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, interactRange, interactLayerMask);
            Debug.DrawRay(origin, dir * interactRange, Color.red, 1f);

            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;
                Debug.Log("Ray가 맞춘 오브젝트: " + target.name);

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
            Debug.Log("Coral Staff를 획득했습니다! 이제 원거리 공격이 가능합니다!");
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

        // 이동 스크립트 비활성화 → 좌우 이동 차단
        Player.Instance.movement.enabled = false;
        rb.linearVelocity = Vector2.zero;   // 잔여 속도 제거
        rb.gravityScale = 0f;         // 중력 제거
        

        if (anim != null)
            anim.SetTrigger("ClimbStart"); // 널 체크 나중에 에니메이션 추가되면 변경

        Debug.Log("사다리에 올라탐");

        StartCoroutine(ResetLadderJustEntered());
    }

    private IEnumerator ResetLadderJustEntered()
    {
        yield return new WaitForSeconds(0.3f);
        ladderJustEntered = false;
    }

    private void ExitLadder()
    {
        isOnLadder = false;
        currentLadder = null;

        Player.Instance.movement.enabled = true;
        rb.gravityScale = 1f;
        

        if (anim != null)
            anim.SetTrigger("ClimbStart"); // 널 체크 나중에 에니메이션 추가되면 변경

        Debug.Log("사다리에서 내려옴");
    }

    public bool IsOnLadder() => isOnLadder;
    public Ladder GetCurrentLadder() => currentLadder;
    public void ForceExitLadder() => ExitLadder();
}
