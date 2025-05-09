using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 180f; // 1초에 180도 회전

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
