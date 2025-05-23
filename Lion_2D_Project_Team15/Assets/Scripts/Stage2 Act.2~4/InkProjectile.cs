using UnityEngine;

public class InkProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 5f;
    public float lifetime = 3f;

    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("먹물 히트!");
            }
            Destroy(gameObject);
        }
    }
}
