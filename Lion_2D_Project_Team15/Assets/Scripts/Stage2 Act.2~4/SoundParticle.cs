using UnityEngine;

public class SoundParticle : MonoBehaviour
{
    public float speed = 1f;
    public float fadeDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private Transform targetWaypoint;
    private float lifetime;
    private bool hasReachedWaypoint = false; // 웨이포인트 도달 여부 플래그

    public void SetTargetWaypoint(Transform waypoint)
    {
        targetWaypoint = waypoint;
        hasReachedWaypoint = false; // 새 웨이포인트 설정 시 플래그 초기화
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lifetime = fadeDuration;
    }

    private void Update()
    {
        if (targetWaypoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            if (!hasReachedWaypoint && Vector3.Distance(transform.position, targetWaypoint.position) < 0.15f)
            {
                Debug.Log($"웨이포인트 {targetWaypoint.name} 도달!");
                hasReachedWaypoint = true; // 한 번만 실행되도록 플래그 설정

                var emitter = Object.FindFirstObjectByType<SoundEmitter>(); // Unity 6 방식
                emitter?.AdvanceWaypoint();
            }
        }

        if (spriteRenderer != null)
        {
            lifetime -= Time.deltaTime;
            float alpha = Mathf.Clamp01(lifetime / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }
    }
}
