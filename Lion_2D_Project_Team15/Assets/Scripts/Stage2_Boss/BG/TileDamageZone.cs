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

        if (collision.GetComponent<move>().isStuck || collision.GetComponent<move>().isKeyInput) return;

        //회오리 순간이동 상태거나 키입력 상태일 때는 return
        //if (Player.Instance.isStuck || Player.Instance.isKeyInput)return;

        if (Time.time - lastDamageTime > damageInterval)
        {
            Debug.Log("붕괴 타일 데미지!");
            // Player.Instance.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}
