using UnityEngine;

public class SoundParticle : MonoBehaviour
{
    public float speed = 1f;
    public float fadeDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private Transform[] waypoints;
    private int currentIndex = 0;
    private float lifetime;
    private Vector3 targetPos;

    // 웨이포인트 경로를 받아서 시작
    public void SetWaypoints(Transform[] points)
    {
        waypoints = points;
        currentIndex = 0;
        if (waypoints != null && waypoints.Length > 0)
            targetPos = waypoints[0].position;
        else
            targetPos = transform.position;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lifetime = fadeDuration;
    }

    private void Update()
    {
        // 웨이포인트 경로 따라 이동
        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                currentIndex++;
                if (currentIndex < waypoints.Length)
                {
                    targetPos = waypoints[currentIndex].position;
                }
                // 마지막 웨이포인트 도달 시 원하는 동작 추가(멈춤, 파괴 등)
            }
        }
        else
        {
            // 경로가 없으면 제자리
        }

        // 페이드 아웃
        if (spriteRenderer != null)
        {
            lifetime -= Time.deltaTime;
            float alpha = Mathf.Clamp01(lifetime / fadeDuration);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
