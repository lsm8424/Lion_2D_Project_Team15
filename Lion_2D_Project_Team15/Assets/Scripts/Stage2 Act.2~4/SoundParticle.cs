using UnityEngine;

public class SoundParticle : MonoBehaviour
{
    public float speed = 1f;
    public float fadeDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection;
    private float lifetime;

    public void SetTarget(Vector3 targetPosition)
    {
        moveDirection = (targetPosition - transform.position).normalized;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lifetime = fadeDuration;
    }

    private void Update()
    {
        // 이동
        transform.position += moveDirection * speed * Time.deltaTime;

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
