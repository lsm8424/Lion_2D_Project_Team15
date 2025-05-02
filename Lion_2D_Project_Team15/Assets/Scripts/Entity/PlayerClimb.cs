using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Player.Instance.interaction.IsOnLadder())
        {
            float vertical = Input.GetAxisRaw("Vertical"); // W/S 키

            rb.linearVelocityX = 0;
            if(Player.Instance.interaction.currentLadder != null)
            {
                Ladder ladder = Player.Instance.interaction.currentLadder.GetComponent<Ladder>();
                float x = ladder.centerX;
                float y = transform.position.y;

                if(transform.position.y > ladder.topExitY)
                    y = ladder.topExitY;

                transform.position = new Vector2(x,y);
            }
                

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
        }
    }
}
