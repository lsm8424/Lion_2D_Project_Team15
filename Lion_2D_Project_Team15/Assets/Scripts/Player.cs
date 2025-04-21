using UnityEngine;

public class Player : Entity
{
    [Header("Player Movement")]
    public float moveSpeed;
    public float jumpForce;
    public bool isGrounded;

    [Header("Components")]
    private Rigidbody rb;
    private Camera mainCam;

    [Header("Combat Stats")]
    public float attackPower;
    public float attackCooldown;
    private float lastAttackTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
    }

    void Update()
    {
        Move();
        HandleJump();
        HandleAttack();
    }

    public override void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void HandleAttack()
    {
        //쿨타임 체크
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            if (anim != null)
            anim.SetTrigger("Attack");

            Debug.Log("플레이어가 공격!");
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
