using UnityEngine;

public class PlayerDamageZone : MonoBehaviour
{
    Vector2 centerPos = new Vector2(200, 50);

    float radius = 20f;
    float damage = 10;
    float damageInterval = 1f;

    float lastDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
            return;

        if (collision.CompareTag("Player"))
        {
            float distance = Vector2.Distance(collision.transform.position, centerPos);
            bool isInZone = distance <= radius;  // 원 안에 있으면 true, 바깥에 있으면 false

            if (!isInZone)  // 원 밖에 있을 때만 damage 처리
            {
                if (Time.time - lastDamageTime > damageInterval)
                {
                    Debug.Log("원형 밖의 존 데미지!");
                    //Player.Instance.TakeDamage(damage);  // 데미지 주기
                    lastDamageTime = Time.time;  // 마지막 데미지 시간 기록
                }
            }
        }
    }

}
