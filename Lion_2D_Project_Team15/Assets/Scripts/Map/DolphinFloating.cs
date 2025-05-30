using UnityEngine;

public class DolphinFloating : MonoBehaviour
{
    public float floatSpeed = 1f; // 상하로 움직이는 속도
    public float floatAmplitude = 0.5f; // 상하 움직임의 범위
    public float horizontalSpeed = 0.5f; // 좌우 움직이는 속도
    public float horizontalAmplitude = 0.3f; // 좌우 움직임의 범위

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 상하로 부드럽게 움직임
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // 좌우로 천천히 움직이는 효과 추가
        float newX = startPosition.x + Mathf.Cos(Time.time * horizontalSpeed) * horizontalAmplitude;

        // 새로운 위치로 업데이트
        transform.position = new Vector3(newX, newY, startPosition.z);
    }
}
