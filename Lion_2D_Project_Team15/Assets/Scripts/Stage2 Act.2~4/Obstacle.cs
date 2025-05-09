using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("장애물 충돌 감지됨");
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Stun();
            }
        }
    }


}
