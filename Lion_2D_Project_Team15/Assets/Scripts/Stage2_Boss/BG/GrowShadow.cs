using UnityEngine;

public class GrowShadow : MonoBehaviour
{
    float growDuration;
    float timer;
    Vector2 targetScale;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / growDuration;

        transform.localScale = Vector2.Lerp(Vector2.zero, targetScale, t);
    }

    public void SetShadow(Vector3 pos,float duration)
    {
        transform.position = pos;
        growDuration = duration;
        targetScale = new Vector2(0.8f, 0.8f);
        transform.localScale = Vector2.zero;
    }
}
