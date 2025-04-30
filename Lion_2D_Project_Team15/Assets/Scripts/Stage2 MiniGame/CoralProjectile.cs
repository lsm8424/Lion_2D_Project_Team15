using UnityEngine;

public class CoralProjectile : MonoBehaviour
{
    public float damage;
    public float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Debug.Log("발사체가 몬스터에 명중!");

            Entity monster = collision.GetComponent<Entity>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
