using System;
using UnityEngine;

public class Stage2_Boss_TrapWarning : MonoBehaviour
{
    private float duration;
    private int trapSize;  //트랩 크기에 따른 스프라이트 크기 조정
    private Action onEnd;

    private float timer;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int trapSize, float duration, Action onEnd)
    {
        this.trapSize = trapSize;
        this.duration = duration;
        this.onEnd = onEnd;
        timer = duration;
        sr.transform.localScale = new Vector3(trapSize*2, trapSize*2, 1);
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
}
