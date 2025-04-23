using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // 상호작용 가능한 거리 (Ray 범위)
    public float interactRange = 2f;

    
    private NPC currentNPC = null; // 현재 상호작용 중인 NPC (대화 대상)
    private bool isTalking = false; // 대화 중인지 여부

    public void HandleInteraction()
    {
        // F 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 현재 대화 중이라면 → 다음 대사로 넘김
            if (isTalking)
            {
                currentNPC?.AdvanceDialogue(); // null이 아니면 AdvanceDialogue() 실행
            }
            else
            {
                // 대화 중이 아니면 → Ray를 쏴서 NPC를 찾음
                RaycastHit hit;

                //플레이어 앞 방향으로 interactRange만큼 Ray 발사
                if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
                {
                    GameObject target = hit.collider.gameObject;

                    // 맞은 오브젝트의 태그가 "NPC" 라면
                    if (target.CompareTag("NPC"))
                    {
                        currentNPC = target.GetComponent<NPC>(); // NPC 스크립트 가져오기
                        if (currentNPC != null)
                        {
                            currentNPC.Interact(); // 대화 시작
                            isTalking = true; // 대화 상태로 전환
                        }
                    }

                    // 추후 아이템 상호작용도 추가 가능
                    
                }
            }
        }
    }

    // 대화 종료 시 호출
    public void EndDialogue()
    {
        isTalking = false; // 대화 상태 종료
        currentNPC = null; // 대화 대상 초기화
    }
}
