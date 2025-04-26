using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // �̵��ӵ�
    public float jumpForce; // ���� ��
    public bool isGrounded; // ���� ��� �ִ��� ����

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMove()
    {
        // WASD �Ǵ� ����Ű �Է� �ޱ�
        float h = Input.GetAxisRaw("Horizontal"); // A,D �Ǵ� ��, ��
        float v = Input.GetAxisRaw("Vertical"); // W,S �Ǵ� ��,��

        // �Է� ���������� ���� ��� (Y���� �����ϰ� ��� �̵�)
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
        // �����̽� Ű �Է� && ���� �ִ� ������ ��츸 ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �������� ���� ����
            isGrounded = false; // ���� ���·� ��ȯ
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ����� Ground �±׸� ������ �ִٸ� ���� ó��
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true; // �ٽ� ���� ���� ���·� ����
    }
}
