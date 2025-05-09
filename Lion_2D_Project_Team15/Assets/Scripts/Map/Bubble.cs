using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 1.0f;

    private float minY;
    private float maxY;
    private Vector3 startPosition;

    public void Setup(float minY, float maxY)
    {
        this.minY = minY;
        this.maxY = maxY;
        startPosition = transform.position;
        ResetPosition();
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y > maxY)
        {
            Destroy(gameObject); // ★ maxY에 도달하면 오브젝트 삭제
        }
    }

    void ResetPosition()
    {
        transform.position = new Vector3(startPosition.x, minY, startPosition.z);
    }
}
