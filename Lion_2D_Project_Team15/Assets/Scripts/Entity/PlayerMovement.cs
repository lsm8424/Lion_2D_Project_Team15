using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // 이동속도
    public float jumpForce; // 점프 힘
    public bool isGrounded; // 땅에 닿아 있는지 여부

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMove()
    {
        // WASD 또는 방향키 입력 받기
        float h = Input.GetAxisRaw("Horizontal"); // A,D 또는 ←, →
        float v = Input.GetAxisRaw("Vertical"); // W,S 또는 ↑,↓

        // 입력 방향으로의 벡터 계산 (Y축은 제외하고 평면 이동)
        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        Vector3 move = moveDir * moveSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    public void HandleJump()
    {
        // 스페이스 키 입력 && 땅에 있는 상태일 경우만 점프 가능
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 위쪽으로 힘을 가함
            isGrounded = false; // 공중 상태로 전환
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 대상이 Ground 태그를 가지고 있다면 착지 처리
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true; // 다시 점프 간으 상태로 설정
    }
}
