using UnityEngine;
using System.Collections;

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

    private bool ladderJustEntered = false; // 중복 상호작용 방지용

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    public void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 사다리 타자마자 바로 Exit 되는 것 방지
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

            Vector2 direction = Player.Instance.movement.facingRight ? Vector2.right : Vector2.left;
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
                else if (target.GetComponent<ParentDolphin>() != null)
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
                else if (target.CompareTag("Item"))
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

        // 이동 막기 → 사다리 이동만 가능하게
        Player.Instance.movement.enabled = false;

        rb.linearVelocity = Vector2.zero; // 잔여속도 제거
        rb.gravityScale = 0f;       // 중력 제거

        if (anim != null)
            anim.SetTrigger("ClimbStart");

        Debug.Log("사다리에 올라탐");
    }

    private void ExitLadder()
    {
        isOnLadder = false;
        currentLadder = null;

        Player.Instance.movement.enabled = true;
        rb.gravityScale = 1f; // 중력 복구

        if (anim != null)
            anim.SetTrigger("ClimbEnd");

        Debug.Log("사다리에서 내려옴");
    }


    private IEnumerator ResetLadderJustEntered()
    {
        yield return new WaitForSeconds(0.5f); // 최소 0.5초 기다림
        ladderJustEntered = false;
    }

    public bool IsOnLadder() => isOnLadder;
    public Ladder GetCurrentLadder() => currentLadder;
    public void ForceExitLadder() => ExitLadder();
}
