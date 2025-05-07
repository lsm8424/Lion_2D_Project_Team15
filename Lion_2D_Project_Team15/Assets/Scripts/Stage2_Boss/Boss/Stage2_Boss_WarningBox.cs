using System;
using UnityEngine;

public class Stage2_Boss_WarningBox : MonoBehaviour
{
    private Transform player;
    private float duration;
    private Vector3 startPos;//초기 좌표
    private int count = 0; // 지렁이 갯수에 따른 스프라이트 크기 조정
    private Action onEnd;

    private float timer;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void Initialize(Transform player, Vector3 pos, int count, float duration, Action onEnd)
    {
        this.player = player;
        this.startPos = pos;
        this.count = count;
        this.duration = duration;
        this.onEnd = onEnd;
        timer = duration;

        sr.transform.localScale = new Vector3(sr.transform.localScale.x, count, 1);
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 위치 이동 (플레이어 방향으로)
        transform.position = startPos + dir;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

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
