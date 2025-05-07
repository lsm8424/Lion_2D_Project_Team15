using UnityEngine;

public class Stage2_Boss_TrapSkill : MonoBehaviour
{
    private float lifeTime;
    private float trapSize;
    private int keyInputCount;
    private float keyDuration;
    private float damage;

    private bool triggerEnter = false;

    void Start()
    {
        transform.localScale = new Vector3(trapSize, trapSize, 1); // Set the size of the wave
    }

    void Update()
    {
        //플레이어가 트랩에 들어오지 않았을 때만 시간 감소
        if (!triggerEnter)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject); // Destroy the wave after its lifetime
            }
        }
    }

    public void SetWave(float lifetime, float trapsize, float damage, int keyinputcount, float time)
    {
        lifeTime = lifetime;
        trapSize = trapsize;
        this.damage = damage;
        keyInputCount = keyinputcount;
        keyDuration = time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerEnter) return;

        if (collision.CompareTag("Player"))
        {
            triggerEnter = true;

            KeyInputManager.Instance.StartKeyInput(keyInputCount, keyDuration, () =>
            {
                // 성공 시: 속도 원복

                Debug.Log("Success!"); // 성공 시 처리
                Destroy(gameObject); // 트랩 제거

            },
             () =>
             {
                 // 실패 시: 속도 원복 + 데미지 + 스턴 등

                 Debug.Log("Fail!"); // 실패 시 처리
                 Destroy(gameObject); // 트랩 제거
             });
        }
    }

}
