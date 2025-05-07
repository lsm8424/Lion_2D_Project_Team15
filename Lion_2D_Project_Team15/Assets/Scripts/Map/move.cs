using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(xInput, yInput).normalized; // 방향 벡터 정규화

        rb.linearVelocity = dir * speed;

    }

}
