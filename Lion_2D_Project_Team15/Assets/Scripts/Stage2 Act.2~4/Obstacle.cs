using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌 감지됨!");

            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.Stun();
            }
        }
    }

}
