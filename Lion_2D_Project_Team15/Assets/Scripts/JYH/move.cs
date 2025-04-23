using UnityEngine;

public class move : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocityY);
    }
}
