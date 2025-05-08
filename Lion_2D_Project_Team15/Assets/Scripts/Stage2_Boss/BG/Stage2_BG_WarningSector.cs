using System;
using UnityEngine;

public class Stage2_BG_WarningSector : MonoBehaviour
{
    private float duration;
    private Action onEnd;

    private float timer;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        // 깜빡이기 효과
        float alpha = Mathf.PingPong(Time.time * 5f, 1f);
        sr.color = new Color(1f, 0f, 0f, alpha);

        // 타이머
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            onEnd?.Invoke();
            Destroy(gameObject);
        }
    }

    public void Initialize(float rot, float duration, Action onend)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
        this.duration = duration;
        this.onEnd = onend;
        timer = duration;
    }

}
