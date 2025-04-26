using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    public int currentMap = 0;
    public Text text;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocityY);

        text.text = "ÇöÀç ¸Ê : " + GetCurrentMapName(currentMap);
    }

    private string GetCurrentMapName(int currentmap)
    {
        switch (currentmap)
        {
            case 0:
                return "Ã³À½ ±ú¾î³µÀ» ¶§";
            case 1:
                return "µµ¼­°ü";
            case 2:
                return "È£¼ö°ø¿ø°°Àº ´À³¦";
            default:
                return "Unknown Map";
        }
    }
}
