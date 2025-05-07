using UnityEngine;

public class TileDamageZone : MonoBehaviour
{
    private float damage = 10f;
    private float damageInterval = 1f;
    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (Time.time - lastDamageTime > damageInterval)
        {
            Debug.Log("붕괴 타일 데미지!");
            // Player.Instance.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}
